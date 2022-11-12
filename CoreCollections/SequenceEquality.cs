using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Static helper methods relating to sequence equality of <see cref="IEnumerable{T}"/> instances.
/// </summary>
public static class SequenceEquality
{
    #region Methods
    #region Comparers
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
    public static NestedEqualityComparer<IEnumerable<T>, T> EnumerableComparer<T>()
        => new EnumerableComparerType<T>();

    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="ImmutableArray{T}"/> instances based on sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare default values as equal, and default and non-default values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<ImmutableArray<T>, T> ImmutableArrayComparer<T>()
        => new ImmutableArrayComparerType<T>();

    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="IndexSelectionList{T}"/> instances based on sequence equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<IndexSelectionList<T>, T> IndexSelectionList<T>()
        => new IndexSelectionListComparerType<T>();
    #endregion

    #region GetHashCode
    /// <summary>
    /// Gets a hash code for the <see cref="IEnumerable{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to compare elements of type <typeparamref name="T"/>, or
    /// <see langword="null"/> to use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> was <see langword="null"/>.</exception>
    public static int GetHashCode<T>(IEnumerable<T> items, IEqualityComparer<T>? nestedComparer = null)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Gets a hash code for the <see cref="ImmutableArray{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to compare elements of type <typeparamref name="T"/>, or
    /// <see langword="null"/> to use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="items"/> was default.</exception>
    public static int GetHashCode<T>(ImmutableArray<T> items, IEqualityComparer<T>? nestedComparer = null)
    {
        if (items.IsDefault) throw new StructArgumentDefaultException(nameof(items));
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in items) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }
    #endregion
    #endregion

    #region Types
    private sealed class EnumerableComparerType<T> : NestedEqualityComparer<IEnumerable<T>, T>
    {
        /// <inheritdoc/>
        public override bool Equals(
            [AllowNull] IEnumerable<T> x, [AllowNull] IEnumerable<T> y, IEqualityComparer<T> nestedComparer)
        {
            // Error checking
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));

            // Remove null cases
            if (x is null) return y is null;
            else if (y is null) return false;

            return x.SequenceEqual(y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] IEnumerable<T> obj, IEqualityComparer<T> nestedComparer)
        {
            // Error checking
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            else if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));

            return SequenceEquality.GetHashCode(obj, nestedComparer);
        }
    }

    private sealed class ImmutableArrayComparerType<T> : NestedEqualityComparer<ImmutableArray<T>, T>
    {
        /// <inheritdoc/>
        public override bool Equals(
            [AllowDefault] ImmutableArray<T> x, [AllowDefault] ImmutableArray<T> y,
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
            [DisallowDefault, DisallowNull] ImmutableArray<T> obj, IEqualityComparer<T> nestedComparer)
        {
            // Error checking
            if (nestedComparer is null) throw new ArgumentNullException(nameof(nestedComparer));

            return SequenceEquality.GetHashCode(obj, nestedComparer);
        }
    }

    private sealed class IndexSelectionListComparerType<T> : NestedEqualityComparer<IndexSelectionList<T>, T>
    {
        /// <inheritdoc/>
        public override bool Equals(
            [AllowNull] IndexSelectionList<T> x, [AllowNull] IndexSelectionList<T> y, IEqualityComparer<T> nestedComparer)
            => x is null ? y is null : x.SequenceEqual(y, nestedComparer);

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] IndexSelectionList<T> obj, IEqualityComparer<T> nestedComparer)
            => obj.GetSequenceHashCode(nestedComparer);
    }
    #endregion
}
