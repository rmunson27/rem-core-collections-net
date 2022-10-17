using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Enumeration;

/// <summary>
/// Static helper methods relating to sequence equality of <see cref="IEnumerable{T}"/> instances.
/// </summary>
public static class SequenceEquality
{
    /// <summary>
    /// Creates an <see cref="EqualityComparer{T}"/> that can compare <see cref="IEnumerable{T}"/> instances based on
    /// sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare <see langword="null"/> values as equal, and <see langword="null"/> and
    /// non-<see langword="null"/> values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EqualityComparer<IEnumerable<T>> EnumerableComparer<T>()
        => new EnumerableComparerType<T>();

    /// <summary>
    /// Creates an <see cref="EqualityComparer{T}"/> that can compare <see cref="ImmutableArray{T}"/> instances based
    /// on sequence equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EqualityComparer<ImmutableArray<T>> ImmutableArrayComparer<T>()
        => new ImmutableArrayComparerType<T>();

    /// <summary>
    /// Gets a hash code for the <see cref="IEnumerable{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> was <see langword="null"/>.</exception>
    public static int GetHashCode<T>(IEnumerable<T> items)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));

        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(item);
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Gets a hash code for the <see cref="ImmutableArray{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static int GetHashCode<T>(ImmutableArray<T> items)
    {
        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(item);
        return hashCode.ToHashCode();
    }

    private sealed class EnumerableComparerType<T> : EqualityComparer<IEnumerable<T>>
    {
        /// <inheritdoc/>
        public override bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (x is null) return y is null;
            else if (y is null) return false;
            else return x.SequenceEqual(y);
        }

        /// <inheritdoc/>
        public override int GetHashCode(IEnumerable<T> obj)
        {
            return SequenceEquality.GetHashCode(obj);
        }
    }

    private sealed class ImmutableArrayComparerType<T> : EqualityComparer<ImmutableArray<T>>
    {
        /// <inheritdoc/>
        public override bool Equals(ImmutableArray<T> x, ImmutableArray<T> y)
        {
            return x.SequenceEqual(y);
        }

        /// <inheritdoc/>
        public override int GetHashCode(ImmutableArray<T> obj)
        {
            return SequenceEquality.GetHashCode(obj);
        }
    }
}
