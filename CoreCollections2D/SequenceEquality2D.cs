using Rem.Core.Attributes;
using Rem.Core.Collections;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D;

/// <summary>
/// Provides methods for computing equality and hash codes for 2-dimensional sequences.
/// </summary>
public static class SequenceEquality2D
{
    #region Methods
    #region Comparers
    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="ReadOnly2DArray{T}"/> instances based on 2-dimensional sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare default values as equal, and default and non-default values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<ReadOnly2DArray<T>, T> ReadonlyArrayComparer<T>()
        => new ReadonlyArrayComparerType<T>();

    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare 2-dimensional arrays of
    /// a given type based on 2-dimensional sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare <see langword="null"/> values as equal, and <see langword="null"/> and
    /// non-<see langword="null"/> values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static NestedEqualityComparer<T[,], T> ArrayComparer<T>() => new ArrayComparerType<T>();
    #endregion

    #region Equals
    /// <summary>
    /// Determines if the 2-dimensional arrays passed in are equal according to their 2-dimensional structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to compare elements of type <typeparamref name="T"/>, or
    /// <see langword="null"/> to use the default comparer.
    /// </param>
    /// <returns></returns>
    public static bool Equals<T>(
        ReadOnly2DArray<T> x, ReadOnly2DArray<T> y, IEqualityComparer<T>? nestedComparer = null)
        => Equals(x._array, y._array, nestedComparer);

    /// <inheritdoc cref="Equals{T}(ReadOnly2DArray{T}, ReadOnly2DArray{T}, IEqualityComparer{T}?)"/>
    public static bool Equals<T>(ReadOnly2DArray<T> x, T[,]? y, IEqualityComparer<T>? nestedComparer = null)
        => Equals(x._array, y, nestedComparer);

    /// <inheritdoc cref="Equals{T}(ReadOnly2DArray{T}, ReadOnly2DArray{T}, IEqualityComparer{T}?)"/>
    public static bool Equals<T>(T[,]? x, ReadOnly2DArray<T> y, IEqualityComparer<T>? nestedComparer = null)
        => Equals(x, y._array, nestedComparer);

    /// <inheritdoc cref="Equals{T}(ReadOnly2DArray{T}, ReadOnly2DArray{T}, IEqualityComparer{T}?)"/>
    public static bool Equals<T>(T[,]? x, T[,]? y, IEqualityComparer<T>? nestedComparer = null)
    {
        // Default the comparer if necessary
        nestedComparer ??= EqualityComparer<T>.Default;

        // Eliminate null cases
        if (x is null) return y is null;
        else if (y is null) return false;

        // Eliminate the case of mismatching dimensions
        if (x.GetLength(0) != y.GetLength(0) || x.GetLength(1) != y.GetLength(1)) return false;

        return Array2D.Enumerate(x).SequenceEqual(Array2D.Enumerate(y), nestedComparer);
    }
    #endregion

    #region GetHashCode
    /// <summary>
    /// Gets a hash code for the <see cref="ReadOnly2DArray{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to compare elements of type <typeparamref name="T"/>, or
    /// <see langword="null"/> to use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="items"/> was default.</exception>
    public static int GetHashCode<T>(
        [NonDefaultableStruct] ReadOnly2DArray<T> items, IEqualityComparer<T>? nestedComparer = null)
    {
        if (items.IsDefault) throw new StructArgumentDefaultException(nameof(items));
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        hashCode.Add(items.RowCount);
        hashCode.Add(items.ColumnCount);
        foreach (var item in items._array) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// Gets a hash code for the <see cref="ReadOnly2DArray{T}"/> passed in based on its structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="nestedComparer">
    /// An <see cref="EqualityComparer{T}"/> to use to compare elements of type <typeparamref name="T"/>, or
    /// <see langword="null"/> to use the default comparer.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"><paramref name="items"/> was default.</exception>
    public static int GetHashCode<T>(T[,] items, IEqualityComparer<T>? nestedComparer = null)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        nestedComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        hashCode.Add(items.GetLength(0));
        hashCode.Add(items.GetLength(1));
        foreach (var item in items) hashCode.Add(nestedComparer.GetHashCode(item!));
        return hashCode.ToHashCode();
    }
    #endregion
    #endregion

    #region Types
    private sealed class ArrayComparerType<T> : NestedEqualityComparer<T[,], T>
    {
        /// <inheritdoc/>
        public override bool Equals(T[,]? x, T[,]? y, IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();
            return Equals<T>(x, y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode(T[,] obj, IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();
            return GetHashCode<T>(obj, nestedComparer);
        }
    }

    private sealed class ReadonlyArrayComparerType<T> : NestedEqualityComparer<ReadOnly2DArray<T>, T>
    {
        /// <inheritdoc/>
        public override bool Equals(
            [AllowDefault] ReadOnly2DArray<T> x, [AllowDefault] ReadOnly2DArray<T> y,
            IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();
            return Equals<T>(x, y, nestedComparer);
        }

        /// <inheritdoc/>
        public override int GetHashCode(ReadOnly2DArray<T> obj, IEqualityComparer<T>? nestedComparer)
        {
            nestedComparer = nestedComparer.DefaultIfNull();
            return GetHashCode<T>(obj, nestedComparer);
        }
    }
    #endregion
}
