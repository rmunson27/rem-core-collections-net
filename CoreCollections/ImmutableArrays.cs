using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Extension methods and other static functionality for the <see cref="ImmutableArray{T}"/> struct.
/// </summary>
public static class ImmutableArrays
{
    /// <summary>
    /// Maps a selector over the current <see cref="ImmutableArray{T}"/>, returning a new array containing the results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> was <see langword="null"/>.</exception>
    public static U[] SelectArray<T, U>([NonDefaultableStruct] this ImmutableArray<T> array, Func<T, U> selector)
    {
        if (array.IsDefault) throw new StructArgumentDefaultException(nameof(array));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var uArray = new U[array.Length];
        for (int i = 0; i < array.Length; i++) uArray[i] = selector(array[i]);
        return uArray;
    }

    #region Sequence Equality
    /// <summary>
    /// Determines if the currnet <see cref="ImmutableArray{T}"/> is equal to the specified
    /// <see cref="ReadOnlyArray{T}"/> by comparing their elements.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="array"></param>
    /// <param name="other"></param>
    /// <param name="elementComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare the elements, or <see langword="null"/> to use the
    /// default equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">
    /// Either the current instance or <paramref name="other"/> was the default.
    /// </exception>
    public static bool SequenceEqual<TParent, TChild>(
        [NonDefaultableStruct] this ImmutableArray<TParent> array,
        [NonDefaultableStruct] ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? elementComparer = null)
        where TChild : TParent
    {
        array.ThrowIfArgDefault(nameof(array));
        other.ThrowIfArgDefault(nameof(other));

        if (array.Length != other.Length) return false;

        elementComparer ??= EqualityComparer<TParent>.Default;

        for (int i = 0; i < array.Length; i++) if (!elementComparer.Equals(array[i], other[i])) return false;
        return true;
    }

    #region GetSequenceHashCode
    /// <summary>
    /// Gets a hash code for the current <see cref="ImmutableArray{T}"/> passed in based on the sequence of
    /// its elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to get hash codes for the elements, or <see langword="null"/> to
    /// use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    public static int GetSequenceHashCode<T>(
        [NonDefaultableStruct] this ImmutableArray<T> items, IEqualityComparer<T>? nestedComparer = null)
        => items.GetSequenceHashCode(nestedComparer, nameof(items));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetSequenceHashCode<T>(
        [NonDefaultableStruct] this ImmutableArray<T> items,
        IEqualityComparer<T>? nestedComparer, string itemsParamName)
    {
        if (items.IsDefault) throw new StructArgumentDefaultException(itemsParamName);
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }
    #endregion

    #region Comparer
    /// <summary>
    /// Gets a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="ImmutableArray{T}"/> instances based on sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare default values as equal, and default and non-default values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<ImmutableArray<T>, T> SequenceEqualityComparer<T>()
        => SequenceEqualityComparerType<T>.Instance;

    private sealed class SequenceEqualityComparerType<T> : NestedEqualityComparer<ImmutableArray<T>, T>
    {
        public static readonly SequenceEqualityComparerType<T> Instance = new();

        private SequenceEqualityComparerType() { }

        /// <inheritdoc/>
        public override bool Equals(
            [DefaultableStruct] ImmutableArray<T> x, [DefaultableStruct] ImmutableArray<T> y,
            IEqualityComparer<T> nestedComparer)
        {
            // Error checking
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));

            // Remove default cases
            if (x.IsDefault) return y.IsDefault;
            else if (y.IsDefault) return false;

            return x.SequenceEqual(y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode(
            [NonDefaultableStruct] ImmutableArray<T> obj, IEqualityComparer<T> nestedComparer)
        {
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));
            return obj.GetSequenceHashCode(nestedComparer, nameof(obj));
        }
    }
    #endregion
    #endregion

    #region Helpers
    /// <summary>
    /// Throws an <see cref="StructArgumentDefaultException"/> if the current <see cref="ImmutableArray{T}"/> is
    /// the default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">Thrown if the current instance is the default.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ImmutableArray<T> ThrowIfArgDefault<T>(this ImmutableArray<T> array, string paramName)
        => array.IsDefault ? throw new StructArgumentDefaultException(paramName) : array;
    #endregion
}
