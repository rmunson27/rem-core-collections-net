using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Extension methods and other static functionality for the <see cref="IEnumerable{T}"/> interface.
/// </summary>
public static class Enumerables
{
    /// <summary>
    /// Gets a <see cref="NotSupportedException"/> that can be thrown when an attempt is made to mutate a
    /// readonly collection.
    /// </summary>
    public static NotSupportedException MutationNotSupported => new("Collection is read-only.");

    /// <summary>
    /// Maps a selector over the current <see cref="IEnumerable{T}"/>, returning a new array containing
    /// the results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Either the current instance or <paramref name="selector"/> was <see langword="null"/>.
    /// </exception>
    public static U[] SelectArray<T, U>(this IEnumerable<T> items, Func<T, U> selector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        else return items switch
        {
            null => throw new ArgumentNullException(nameof(items)),
            ICollection<T> coll => coll.SelectItems(selector, coll.Count),
            IReadOnlyCollection<T> coll => coll.SelectItems(selector, coll.Count),
            _ => items.SelectItems(selector, items.Count()),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static U[] SelectItems<T, U>(this IEnumerable<T> items, Func<T, U> selector, int count)
    {
        var uArray = new U[count];
        int index = 0;
        foreach (var item in items) uArray[index++] = selector(item);
        return uArray;
    }

    #region Sequence Equality
    #region GetSequenceHashCode
    /// <summary>
    /// Gets a hash code for the current <see cref="IEnumerable{T}"/> passed in based on the sequence of its elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to get hash codes for the elements, or <see langword="null"/> to
    /// use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">The current instance was <see langword="null"/>.</exception>
    public static int GetSequenceHashCode<T>(this IEnumerable<T> items, IEqualityComparer<T>? nestedComparer = null)
        => items.GetSequenceHashCode(nestedComparer, nameof(items));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetSequenceHashCode<T>(
        this IEnumerable<T> items, IEqualityComparer<T>? nestedComparer, string itemsParamName)
    {
        if (items is null) throw new ArgumentNullException(itemsParamName);
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }
    #endregion

    #region Comparer
    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="IEnumerable{T}"/> instances based on sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare <see langword="null"/> values as equal, and <see langword="null"/> and
    /// non-<see langword="null"/> values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<IEnumerable<T>, T> SequenceEqualityComparer<T>()
        => SequenceEqualityComparerType<T>.Instance;

    private sealed class SequenceEqualityComparerType<T> : NestedEqualityComparer<IEnumerable<T>, T>
    {
        public static readonly SequenceEqualityComparerType<T> Instance = new();

        private SequenceEqualityComparerType() { }

        /// <inheritdoc/>
        public override bool Equals(
            IEnumerable<T>? x, IEnumerable<T>? y, IEqualityComparer<T> nestedComparer)
        {
            // Error checking
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));

            // Remove null cases
            if (x is null) return y is null;
            else if (y is null) return false;

            return x.SequenceEqual(y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode(IEnumerable<T> obj, IEqualityComparer<T> nestedComparer)
        {
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));
            else return obj.GetSequenceHashCode(nestedComparer, nameof(obj));
        }
    }
    #endregion
    #endregion
}
