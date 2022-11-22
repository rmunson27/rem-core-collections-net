using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Static helper methods relating to sequence equality of <see cref="IEnumerable{T}"/> instances.
/// </summary>
/// <remarks>
/// This class is obsolete.
/// Use the analogous static and extension methods in the various collection-type-specific classes provided by this
/// library instead.
/// See the listed classes for the replacement methods, or the methods of this class for their respective replacements.
/// </remarks>
/// <seealso cref="Enumerables"/>
/// <seealso cref="ImmutableArrays"/>
[Obsolete(
    "Replaced with various collection-type-specific static and extension methods. " +
        "See methods for replacement details.")]
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
    [Obsolete(
        "This method will be removed in an upcoming version. " +
        $"Use the {nameof(Enumerables)}.{nameof(Enumerables.SequenceEqualityComparer)} method instead.")]
    public static NestedEqualityComparer<IEnumerable<T>, T> EnumerableComparer<T>()
        => Enumerables.SequenceEqualityComparer<T>();

    /// <summary>
    /// Creates a <see cref="NestedEqualityComparer{TGeneric, TParameter}"/> that can compare
    /// <see cref="ImmutableArray{T}"/> instances based on sequence equality.
    /// </summary>
    /// <remarks>
    /// The returned value will compare default values as equal, and default and non-default values as unequal.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Obsolete(
        "This method will be removed in an upcoming version. " +
        $"Use the {nameof(ImmutableArrays)}.{nameof(ImmutableArrays.SequenceEqualityComparer)} method instead.")]
    public static NestedEqualityComparer<ImmutableArray<T>, T> ImmutableArrayComparer<T>()
        => ImmutableArrays.SequenceEqualityComparer<T>();
    #endregion

    #region Hash Codes
    /// <summary>
    /// Gets a hash code for the specified <see cref="IEnumerable{T}"/> passed in based on the sequence of
    /// its elements.
    /// </summary>
    /// <inheritdoc cref="EnumerableExtensions.GetSequenceHashCode{T}(IEnumerable{T}, IEqualityComparer{T}?)"/>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> was <see langword="null"/>.</exception>
    [Obsolete(
        "This method will be removed in an upcoming version. " +
        $"Use the {nameof(Enumerables)}.{nameof(Enumerables.GetSequenceHashCode)} extension method instead.")]
    public static int GetHashCode<T>(IEnumerable<T> items, IEqualityComparer<T>? nestedComparer = null)
        => items.GetSequenceHashCode(nestedComparer, nameof(items));

    /// <summary>
    /// Gets a hash code for the supplied <see cref="ImmutableArray{T}"/> based on the sequence of its elements.
    /// </summary>
    /// <inheritdoc cref="ImmutableArrayExtensions.GetSequenceHashCode{T}(ImmutableArray{T}, IEqualityComparer{T}?)"/>
    /// <exception cref="StructArgumentDefaultException"><paramref name="items"/> was the default.</exception>
    [Obsolete(
        "This method will be removed in an upcoming version. " +
        $"Use the {nameof(ImmutableArrays)}.{nameof(ImmutableArrays.GetSequenceHashCode)} extension method instead.")]
    public static int GetHashCode<T>(ImmutableArray<T> items, IEqualityComparer<T>? nestedComparer = null)
        => items.GetSequenceHashCode(nestedComparer, nameof(items));
    #endregion
    #endregion
}
