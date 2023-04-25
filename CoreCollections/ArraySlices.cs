using Rem.Core.Attributes;
using Rem.Core.Collections.Enumeration;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Static methods for the <see cref="ArraySlice{T}"/> and <see cref="ReadOnlyArraySlice{T}"/> types.
/// </summary>
public static class ArraySlice
{
    #region Factory Methods
    #region ArraySlice
    /// <summary>
    /// Creates a new <see cref="ArraySlice{T}"/> wrapping the entirety of the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being wrapped.</typeparam>
    /// <param name="array">The array to wrap.</param>
    /// <returns>A new <see cref="ArraySlice{T}"/> wrapping <paramref name="array"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    public static ArraySlice<T> Create<T>(T[] array) => new(array);

    /// <summary>
    /// Creates a new <see cref="ArraySlice{T}"/> containing the elements of the given array starting at the
    /// specified offset.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="offset">The offset into the array at which to start taking elements.</param>
    /// <returns>
    /// A new <see cref="ArraySlice{T}"/> containing the elements of <paramref name="array"/> starting
    /// at <paramref name="offset"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> was negative or out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ArraySlice<T> Create<T>(T[] array, long offset) => new(array, offset);

    /// <summary>
    /// Creates a new <see cref="ArraySlice{T}"/> containing the specified range of elements from the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="offset">The offset into the array at which to start taking elements.</param>
    /// <param name="count">The number of elements to take.</param>
    /// <returns>
    /// A new <see cref="ArraySlice{T}"/> containing the elements of <paramref name="array"/> specified by
    /// <paramref name="offset"/> and <paramref name="count"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> was negative, or the range described by
    /// <paramref name="offset"/> or <paramref name="count"/> was out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ArraySlice<T> Create<T>(T[] array, long offset, long count) => new(array, offset, count);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="Create{T}(T[], LongRange)"/>
    public static ArraySlice<T> Create<T>(T[] array, Range range) => new(array, range);
#endif

    /// <summary>
    /// Creates a new <see cref="ArraySlice{T}"/> containing the specified range of elements from the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="range">A range specifying the elements to take.</param>
    /// <returns>
    /// A new <see cref="ArraySlice{T}"/> containing the elements of <paramref name="array"/> specified
    /// by <paramref name="range"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="range"/> was degenerate or out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ArraySlice<T> Create<T>(T[] array, LongRange range) => new(array, range);
    #endregion

    #region ReadOnlyArraySlice
    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> wrapping the entirety of the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being wrapped.</typeparam>
    /// <param name="array">The array to wrap.</param>
    /// <returns>A new <see cref="ReadOnlyArraySlice{T}"/> wrapping <paramref name="array"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    public static ReadOnlyArraySlice<T> CreateReadOnly<T>(T[] array) => new(array);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing the elements of the given array starting at the
    /// specified offset.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="offset">The offset into the array at which to start taking elements.</param>
    /// <returns>
    /// A new <see cref="ReadOnlyArraySlice{T}"/> containing the elements of <paramref name="array"/> starting
    /// at <paramref name="offset"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> was negative or out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ReadOnlyArraySlice<T> CreateReadOnly<T>(T[] array, long offset) => new(array, offset);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing the specified range of elements from the
    /// given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="offset">The offset into the array at which to start taking elements.</param>
    /// <param name="count">The number of elements to take.</param>
    /// <returns>
    /// A new <see cref="ReadOnlyArraySlice{T}"/> containing the elements of <paramref name="array"/> specified by
    /// <paramref name="offset"/> and <paramref name="count"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> was negative, or the range described by
    /// <paramref name="offset"/> or <paramref name="count"/> was out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ReadOnlyArraySlice<T> CreateReadOnly<T>(T[] array, long offset, long count)
        => new(array, offset, count);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="CreateReadOnly{T}(T[], LongRange)"/>
    public static ReadOnlyArraySlice<T> CreateReadOnly<T>(T[] array, Range range) => new(array, range);
#endif

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing the specified range of elements from the
    /// given array.
    /// </summary>
    /// <typeparam name="T">The type of elements of the array being sliced.</typeparam>
    /// <param name="array">The array to take a slice of.</param>
    /// <param name="range">A range specifying the elements to take.</param>
    /// <returns>
    /// A new <see cref="ReadOnlyArraySlice{T}"/> containing the elements of <paramref name="array"/> specified
    /// by <paramref name="range"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="range"/> was degenerate or out of bounds for <paramref name="array"/>.
    /// </exception>
    public static ReadOnlyArraySlice<T> CreateReadOnly<T>(T[] array, LongRange range) => new(array, range);
    #endregion
    #endregion

    #region Extension Methods
    #region LINQ
    #region ArraySlice
    #region SequenceEqual
    public static bool SequenceEqual<TParent, TChild>(this IEnumerable<TParent> first,
                                                      [NonDefaultableStruct] in ArraySlice<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.SequenceEqual(second.Core, comparer);

    /// <inheritdoc cref="ArraySliceCoreExtensions.SequenceEqual{TParent, TChild}(ReadOnlyArray{TParent}, in ArraySliceCore{TChild}, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] this ReadOnlyArray<TParent> first,
                                                      [NonDefaultableStruct] in ArraySlice<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.SequenceEqual(second.Core, comparer);

    /// <inheritdoc cref="ArraySliceCoreExtensions.SequenceEqual{TParent, TChild}(in ArraySliceCore{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] in this ArraySlice<TParent> first,
                                                      [NonDefaultableStruct] ReadOnlyArray<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.Core.SequenceEqual(second, comparer);

    /// <inheritdoc cref="ArraySliceCoreExtensions.SequenceEqual{TParent, TChild}(in ArraySliceCore{TParent}, in ArraySliceCore{TChild}, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] in this ArraySlice<TParent> first,
                                                      [NonDefaultableStruct] in ArraySlice<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.Core.SequenceEqual(second.Core, comparer);
    #endregion
    #endregion
    #endregion
    #endregion
}

/// <summary>
/// Extensions for the <see cref="ArraySliceCore{T}"/> struct.
/// </summary>
/// <remarks>
/// This class will provide the core functionality for all array slice type extension methods defined in this file.
/// </remarks>
file static class ArraySliceCoreExtensions
{
    #region LINQ
    #region SequenceEqual
    /// <summary>
    /// Determines if the current <see cref="IEnumerable{T}"/> is equal to a specified array slice.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="first">The current <see cref="IEnumerable{T}"/>.</param>
    /// <param name="second">An array slice to compare to the current <see cref="IEnumerable{T}"/>.</param>
    /// <returns>
    /// Whether or not the current <see cref="IEnumerable{T}"/> is sequence-equal to <paramref name="second"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="first"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<TParent, TChild>(this IEnumerable<TParent> first,
                                                      [NonDefaultableStruct] in ArraySliceCore<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
        where TChild : TParent
        => SpecifiedEnumerable.SequenceEqual<TParent, ArraySliceCore<TChild>, ArraySliceEnumerator<TChild>, TChild>(
            first, in second, comparer);

    /// <summary>
    /// Determines if the current slice is sequence-equal to another <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The element type of the current slice.</typeparam>
    /// <param name="first">The current slice.</param>
    /// <param name="second">An <see cref="IEnumerable{T}"/> to compare to the current slice.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare values of type <typeparamref name="TSource"/>, or
    /// <see langword="null"/> to use the default equality comparer for type <typeparamref name="TSource"/>.
    /// </param>
    /// <returns>Whether or not the current slice is sequence-equal to <paramref name="second"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="second"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<TSource>([NonDefaultableStruct] in this ArraySliceCore<TSource> first,
                                              IEnumerable<TSource> second,
                                              IEqualityComparer<TSource>? comparer)
        => SpecifiedEnumerable.SequenceEqual<ArraySliceCore<TSource>, ArraySliceEnumerator<TSource>, TSource>(
            in first, second, comparer);

    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> is equal to an array slice.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current <see cref="ReadOnlyArray{T}"/>.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the slice.
    /// </typeparam>
    /// <param name="first">The current <see cref="ReadOnlyArray{T}"/>.</param>
    /// <param name="second">
    /// An array slice of element type <typeparamref name="TChild"/> to compare to the
    /// current <see cref="ReadOnlyArray{T}"/>.
    /// </param>
    /// <returns>
    /// Whether or not the current <see cref="ReadOnlyArray{T}"/> is sequence-equal to <paramref name="second"/>.
    /// </returns>
    /// <inheritdoc cref="SequenceEqual{TParent, TChild}(in ArraySliceCore{TParent}, in ArraySliceCore{TChild}, IEqualityComparer{TParent}?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] this ReadOnlyArray<TParent> first,
                                                      [NonDefaultableStruct] in ArraySliceCore<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => SpecifiedEnumerable.SequenceEqual<ReadOnlyArray<TParent>, ArrayEnumerator<TParent>, TParent,
                                             ArraySliceCore<TChild>, ArraySliceEnumerator<TChild>, TChild>(
            in first, in second, comparer);

    /// <summary>
    /// Determines if the current slice is sequence-equal to a <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the <see cref="ReadOnlyArray{T}"/>.
    /// </typeparam>
    /// <param name="second">A <see cref="ReadOnlyArray{T}"/> to compare to the current slice.</param>
    /// <inheritdoc cref="SequenceEqual{TParent, TChild}(in ArraySliceCore{TParent}, in ArraySliceCore{TChild}, IEqualityComparer{TParent}?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] in this ArraySliceCore<TParent> first,
                                                      [NonDefaultableStruct] ReadOnlyArray<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => SpecifiedEnumerable.SequenceEqual<ArraySliceCore<TParent>, ArraySliceEnumerator<TParent>, TParent,
                                             ReadOnlyArray<TChild>, ArrayEnumerator<TChild>, TChild>(
            in first, in second, comparer);

    /// <summary>
    /// Determines if the current slice is sequence-equal to another.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current slice.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the second slice.
    /// </typeparam>
    /// <param name="first">The current slice.</param>
    /// <param name="second">
    /// Another slice of element type <typeparamref name="TChild"/> to compare with the current slice for
    /// sequence equality.
    /// </param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare elements of type <typeparamref name="TParent"/>, or
    /// <see langword="null"/> to use the default comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns>Whether or not the current slice is sequence-equal to <paramref name="second"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<TParent, TChild>([NonDefaultableStruct] in this ArraySliceCore<TParent> first,
                                                      [NonDefaultableStruct] in ArraySliceCore<TChild> second,
                                                      IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => SpecifiedEnumerable.SequenceEqual<ArraySliceCore<TParent>, ArraySliceEnumerator<TParent>, TParent,
                                             ArraySliceCore<TChild>, ArraySliceEnumerator<TChild>, TChild>(
            in first, in second, comparer);
    #endregion
    #endregion
}

/// <summary>
/// Represents a read-only slice of an array, the individual elements of which can be accessed via this struct.
/// </summary>
/// <typeparam name="T">The type of elements of the array.</typeparam>
public readonly struct ReadOnlyArraySlice<T> : IDefaultableStruct, IEnumerable<T>, IList<T>, IReadOnlyList<T>,
                                               IEquatable<ReadOnlyArraySlice<T>>
{
    #region Properties And Indexers
    /// <inheritdoc/>
    public bool IsDefault => Core.IsDefault;

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(long)"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[int index] => ref Core.ElementAt(index);

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(long)"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[long index] => ref Core.ElementAt(index);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Slice(Range)"/>
    public ReadOnlyArraySlice<T> this[Range range] => new(Core.Slice(range));

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(Index))"/>
    public ref readonly T this[Index index] => ref Core.ElementAt(index);
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(LongRange)"/>
    public ReadOnlyArraySlice<T> this[LongRange range] => new(Core.Slice(range));

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(LongIndex))"/>
    public ref readonly T this[LongIndex index] => ref Core.ElementAt(index);

    [DoesNotReturnIfInstanceDefault]
    T IList<T>.this[int index]
    {
        get => Core.ElementAt(index);
        set => throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturnIfInstanceDefault]
    T IReadOnlyList<T>.this[int index] => Core.ElementAt(index);

    /// <inheritdoc cref="ArraySliceCore{T}.Array"/>
    [MaybeDefaultIfInstanceDefault]
    public ReadOnlyArray<T> Array => new(Core.Array);

    /// <inheritdoc cref="ArraySliceCore{T}.Offset"/>
    public int Offset => Core.Offset;

    /// <inheritdoc cref="ArraySliceCore{T}.Count"/>
    public int Count => Core.Count;

    /// <inheritdoc cref="ArraySliceCore{T}.LongOffset"/>
    public long LongOffset => Core.LongOffset;

    /// <inheritdoc cref="ArraySliceCore{T}.LongCount"/>
    public long LongCount => Core.LongCount;

    /// <summary>
    /// The core of this instance.
    /// </summary>
    private readonly ArraySliceCore<T> Core;

    bool ICollection<T>.IsReadOnly => true;
    #endregion

    #region Constructors
    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[])"/>
    public ReadOnlyArraySlice(T[] array) : this(new ArraySliceCore<T>(array)) { }

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], LongRange)"/>
    public ReadOnlyArraySlice(T[] array, LongRange range) : this(new ArraySliceCore<T>(array, range)) { }

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], Range)"/>
    public ReadOnlyArraySlice(T[] array, Range range) : this(new ArraySliceCore<T>(array, range)) { }
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], long)"/>
    public ReadOnlyArraySlice(T[] array, long offset) : this(new ArraySliceCore<T>(array, offset)) { }

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], long, long)"/>
    public ReadOnlyArraySlice(T[] array, long offset, long count) : this(new ArraySliceCore<T>(array, offset, count)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyArraySlice(in ArraySliceCore<T> Core) { this.Core = Core; }
    #endregion

    #region Slicing
    #region Exception-Throwing
    /// <inheritdoc cref="ArraySliceCore{T}.TruncateAt(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TruncateAt(long count) => new(Core.TruncateAt(count));

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Slice(long offset) => new(Core.Slice(offset));

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Slice(Range)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Slice(Range range) => new(Core.Slice(range));
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(long, long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Slice(long start, long count) => new(Core.Slice(start, count));

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Slice(LongRange range) => new(Core.Slice(range));
    #endregion

    #region Clamped
    /// <inheritdoc cref="ArraySliceCore{T}.Skip(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Skip(long count) => new(Core.Skip(count));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipLast(long count) => new(Core.SkipLast(count));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, int, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, int, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, long, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.Take(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(long count) => new(Core.Take(count));

    /// <inheritdoc cref="ArraySliceCore{T}.Take(long, long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(long offset, long count) => new(Core.Take(offset, count));

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Take(Range)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(Range range) => new(Core.Take(range));
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Take(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(LongRange range) => new(Core.Take(range));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, int, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, int, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, long, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeLast(int)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(int count) => new(Core.TakeLast(count));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(long count) => new(Core.TakeLast(count));
    #endregion
    #endregion

    #region GetEnumerator
    /// <inheritdoc cref="ArraySliceCore{T}.GetEnumerator"/>
    public ArraySliceEnumerator<T> GetEnumerator() => new(Core);

    [DoesNotReturnIfInstanceDefault]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Core.GetEnumeratorObject();

    [DoesNotReturnIfInstanceDefault]
    IEnumerator IEnumerable.GetEnumerator() => Core.GetEnumeratorObject();
    #endregion

    #region ICollection and IList Implementations
    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public bool Contains(T item) => Core.Contains(item);

    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public int IndexOf(T item) => Core.IndexOf(item);

    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public void CopyTo(T[] array, int arrayIndex) => Core.CopyTo(array, arrayIndex);

    #region Not Supported
    [DoesNotReturn] bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    [DoesNotReturn] void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    [DoesNotReturn] void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
    [DoesNotReturn] void ICollection<T>.Add(T item) => throw new NotSupportedException();
    [DoesNotReturn] void ICollection<T>.Clear() => throw new NotSupportedException();
    #endregion
    #endregion

    #region Conversions
    /// <inheritdoc cref="ArraySliceCore{T}.FromChild{TChild}(in ArraySliceCore{TChild})"/>
    public static ReadOnlyArraySlice<T> FromChild<TChild>(in ReadOnlyArraySlice<TChild> slice) where TChild : class, T
        => new(ArraySliceCore<T>.FromChild(in slice.Core));

    /// <inheritdoc cref="ArraySliceCore{T}.op_Implicit(T[]?)"/>
    [return: MaybeDefaultIfDefault(nameof(array))]
    public static implicit operator ReadOnlyArraySlice<T>(T[]? array) => new((ArraySliceCore<T>)array);

    /// <summary>
    /// Implicitly converts an <see cref="ArraySlice{T}"/> to a <see cref="ReadOnlyArraySlice{T}"/>.
    /// </summary>
    /// <remarks>Default instances will be mapped to the default.</remarks>
    /// <param name="slice"></param>
    [return: MaybeDefaultIfDefault(nameof(slice))]
    public static implicit operator ReadOnlyArraySlice<T>(in ArraySlice<T> slice) => new(in slice.Core);

    /// <summary>
    /// Implicitly converts a <see cref="ReadOnlyArray{T}"/> to a slice representing the entire array.
    /// </summary>
    /// <remarks>Default instances will be mapped to the default.</remarks>
    /// <param name="array"></param>
    [return: MaybeDefaultIfDefault(nameof(array))]
    public static implicit operator ReadOnlyArraySlice<T>(ReadOnlyArray<T> array)
        => new((ArraySliceCore<T>)array._array);

    /// <inheritdoc cref="ArraySliceCore{T}.op_Implicit(ArraySegment{T})"/>
    public static implicit operator ReadOnlyArraySlice<T>(ArraySegment<T> segment) => new((ArraySliceCore<T>)segment);

    /// <inheritdoc cref="ArraySliceCore{T}.implicit operator ReadOnlySpan{T}(in ArraySliceCore{T})"/>
    public static implicit operator ReadOnlySpan<T>(in ReadOnlyArraySlice<T> slice) => slice.Core;
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the two slices are equal.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator ==(ReadOnlyArraySlice<T> first, ReadOnlyArraySlice<T> second) => first.Equals(second);

    /// <summary>
    /// Determines if the two instances are not equal.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator !=(ReadOnlyArraySlice<T> first, ReadOnlyArraySlice<T> second) => !first.Equals(second);

    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is ReadOnlyArraySlice<T> other && Equals(other);

    /// <inheritdoc cref="ArraySliceCore{T}.Equals(in ArraySliceCore{T})"/>
    public bool Equals(ReadOnlyArraySlice<T> other) => Core.Equals(in other.Core);

    /// <inheritdoc cref="ArraySliceCore{T}.GetHashCode()"/>
    public override int GetHashCode() => Core.GetHashCode();
    #endregion
}

/// <summary>
/// Represents a slice of an array, the individual elements of which can be accessed or set via this struct.
/// </summary>
/// <typeparam name="T">The type of elements of the array.</typeparam>
public readonly struct ArraySlice<T> : IDefaultableStruct, IEnumerable<T>, IList<T>, IReadOnlyList<T>
{
    #region Properties And Indexers
    /// <inheritdoc/>
    public bool IsDefault => Core.IsDefault;

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(long)"/>
    [DoesNotReturnIfInstanceDefault]
    public ref T this[int index] => ref Core.ElementAt(index);

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(long)"/>
    [DoesNotReturnIfInstanceDefault]
    public ref T this[long index] => ref Core.ElementAt(index);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Slice(Range)"/>
    public ArraySlice<T> this[Range range] => new(Core.Slice(range));

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(Index))"/>
    public ref T this[Index index] => ref Core.ElementAt(index);
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(LongRange)"/>
    public ArraySlice<T> this[LongRange range] => new(Core.Slice(range));

    /// <inheritdoc cref="ArraySliceCore{T}.ElementAt(LongIndex))"/>
    public ref T this[LongIndex index] => ref Core.ElementAt(index);

    [DoesNotReturnIfInstanceDefault]
    T IList<T>.this[int index]
    {
        get => Core.ElementAt(index);
        set => Core.ElementAt(index) = value;
    }

    [DoesNotReturnIfInstanceDefault]
    T IReadOnlyList<T>.this[int index] => Core.ElementAt(index);

    /// <inheritdoc cref="ArraySliceCore{T}.Array"/>
    [MaybeDefaultIfInstanceDefault]
    public T[] Array => Core.Array;

    /// <inheritdoc cref="ArraySliceCore{T}.Offset"/>
    public int Offset => Core.Offset;

    /// <inheritdoc cref="ArraySliceCore{T}.Count"/>
    public int Count => Core.Count;

    /// <inheritdoc cref="ArraySliceCore{T}.LongOffset"/>
    public long LongOffset => Core.LongOffset;

    /// <inheritdoc cref="ArraySliceCore{T}.LongCount"/>
    public long LongCount => Core.LongCount;

    /// <summary>
    /// The core of this instance.
    /// </summary>
    internal readonly ArraySliceCore<T> Core;

    bool ICollection<T>.IsReadOnly => false;
    #endregion

    #region Constructors
    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[])"/>
    public ArraySlice(T[] array) : this(new ArraySliceCore<T>(array)) { }

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], LongRange)"/>
    public ArraySlice(T[] array, LongRange range) : this(new ArraySliceCore<T>(array, range)) { }

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], Range)"/>
    public ArraySlice(T[] array, Range range) : this(new ArraySliceCore<T>(array, range)) { }
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], long)"/>
    public ArraySlice(T[] array, long offset) : this(new ArraySliceCore<T>(array, offset)) { }

    /// <inheritdoc cref="ArraySliceCore{T}.ArraySliceCore(T[], long, long)"/>
    public ArraySlice(T[] array, long offset, long count) : this(new ArraySliceCore<T>(array, offset, count)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ArraySlice(ArraySliceCore<T> Core) { this.Core = Core; }
    #endregion

    #region Slicing
    #region Exception-Throwing
    /// <inheritdoc cref="ArraySliceCore{T}.TruncateAt(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TruncateAt(long count) => new(Core.TruncateAt(count));

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Slice(long offset) => new(Core.Slice(offset));

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Slice(Range)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Slice(Range range) => new(Core.Slice(range));
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(long, long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Slice(long start, long count) => new(Core.Slice(start, count));

    /// <inheritdoc cref="ArraySliceCore{T}.Slice(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Slice(LongRange range) => new(Core.Slice(range));
    #endregion

    #region Clamped
    /// <inheritdoc cref="ArraySliceCore{T}.Skip(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Skip(long count) => new(Core.Skip(count));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> SkipLast(long count) => new(Core.SkipLast(count));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> SkipWhile(Func<T, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, int, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> SkipWhile(Func<T, int, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> SkipWhile(Func<T, long, bool> predicate) => new(Core.SkipWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.Take(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Take(long count) => new(Core.Take(count));

    /// <inheritdoc cref="ArraySliceCore{T}.Take(long, long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Take(long offset, long count) => new(Core.Take(offset, count));

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ArraySliceCore{T}.Take(Range)"/>
    public ArraySlice<T> Take(Range range) => new(Core.Take(range));
#endif

    /// <inheritdoc cref="ArraySliceCore{T}.Take(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> Take(LongRange range) => new(Core.Take(range));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TakeWhile(Func<T, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, int, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TakeWhile(Func<T, int, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TakeWhile(Func<T, long, bool> predicate) => new(Core.TakeWhile(predicate));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeLast(int)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TakeLast(int count) => new(Core.TakeLast(count));

    /// <inheritdoc cref="ArraySliceCore{T}.TakeLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySlice<T> TakeLast(long count) => new(Core.TakeLast(count));
    #endregion
    #endregion

    #region GetEnumerator
    /// <inheritdoc cref="ArraySliceCore{T}.GetEnumerator"/>
    public ArraySliceEnumerator<T> GetEnumerator() => new(Core);

    [DoesNotReturnIfInstanceDefault]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Core.GetEnumeratorObject();

    [DoesNotReturnIfInstanceDefault]
    IEnumerator IEnumerable.GetEnumerator() => Core.GetEnumeratorObject();
    #endregion

    #region ICollection and IList Implementations
    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public bool Contains(T item) => Core.Contains(item);

    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public int IndexOf(T item) => Core.IndexOf(item);

    /// <inheritdoc cref="ArraySliceCore{T}.Contains(T)"/>
    [DoesNotReturnIfInstanceDefault]
    public void CopyTo(T[] array, int arrayIndex) => Core.CopyTo(array, arrayIndex);

    #region Not Supported
    [DoesNotReturn] bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    [DoesNotReturn] void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    [DoesNotReturn] void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
    [DoesNotReturn] void ICollection<T>.Add(T item) => throw new NotSupportedException();
    [DoesNotReturn] void ICollection<T>.Clear() => throw new NotSupportedException();
    #endregion
    #endregion

    #region Conversions
    /// <remarks>
    /// It should be noted that any attempt to set an index of the array to a value of type <typeparamref name="T"/>
    /// that is not also a value of type <typeparamref name="TChild"/> will result in an
    /// <see cref="ArrayTypeMismatchException"/> being thrown.
    /// </remarks>
    /// <inheritdoc cref="ArraySliceCore{T}.FromChild{TChild}(in ArraySliceCore{TChild})"/>
    public static ArraySlice<T> FromChild<TChild>(in ArraySlice<TChild> slice) where TChild : class, T
        => new(ArraySliceCore<T>.FromChild(in slice.Core));

    /// <inheritdoc cref="ArraySliceCore{T}.op_Implicit(T[]?)"/>
    [return: MaybeDefaultIfDefault(nameof(array))]
    public static implicit operator ArraySlice<T>(T[]? array) => new((ArraySliceCore<T>)array);

    /// <inheritdoc cref="ArraySliceCore{T}.op_Implicit(ArraySegment{T})"/>
    public static implicit operator ArraySlice<T>(ArraySegment<T> segment) => new((ArraySliceCore<T>)segment);

    /// <inheritdoc cref="ArraySliceCore{T}.implicit operator Span{T}(in ArraySliceCore{T})"/>
    public static implicit operator Span<T>(in ArraySlice<T> slice) => slice.Core;

    /// <inheritdoc cref="ArraySliceCore{T}.implicit operator ReadOnlySpan{T}(in ArraySliceCore{T})"/>
    public static implicit operator ReadOnlySpan<T>(in ArraySlice<T> slice) => slice.Core;

    /// <inheritdoc cref="ArraySliceCore{T}.implicit operator ArraySegment{T}(in ArraySliceCore{T})"/>
    public static implicit operator ArraySegment<T>(in ArraySlice<T> slice) => (ArraySegment<T>)slice.Core;
    #endregion

    #region Equality
    /// <summary>
    /// Determines if the two slices are equal.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator ==(ArraySlice<T> first, ArraySlice<T> second) => first.Equals(second);

    /// <summary>
    /// Determines if the two instances are not equal.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator !=(ArraySlice<T> first, ArraySlice<T> second) => !first.Equals(second);

    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is ArraySlice<T> other && Equals(other);

    /// <inheritdoc cref="ArraySliceCore{T}.Equals(ArraySliceCore{T})"/>
    public bool Equals(ArraySlice<T> other) => Core.Equals(other.Core);

    /// <inheritdoc cref="ArraySliceCore{T}.GetHashCode()"/>
    public override int GetHashCode() => Core.GetHashCode();
    #endregion
}

#region Common Functionality
/// <summary>
/// A struct that can be used to traverse an array slice.
/// </summary>
public struct ArraySliceEnumerator<T> : IDefaultableStruct, IEnumerator<T>
{
    private const long NotStartedIndex = -1;
    private const long AlreadyFinishedIndex = -2;

    /// <inheritdoc/>
    public bool IsDefault => Slice.IsDefault;

    object? IEnumerator.Current => Current;

    /// <summary>
    /// Gets the current element of the enumeration.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The enumeration has not yet started, or is already finished.
    /// </exception>
    [DoesNotReturnIfInstanceDefault]
    public T Current => CurrentIndex switch
    {
        NotStartedIndex => throw Enumerators.NotStartedException,
        AlreadyFinishedIndex => throw Enumerators.AlreadyFinishedException,
        _ => Slice.ElementAt(CurrentIndex),
    };

    private long CurrentIndex { get; set; }

    private ArraySliceCore<T> Slice { get; }

    /// <summary>
    /// Constructs a new slice enumerator wwapping the slice core passed in.
    /// </summary>
    /// <param name="slice"></param>
    internal ArraySliceEnumerator(ArraySliceCore<T> slice)
    {
        Slice = slice;
        CurrentIndex = NotStartedIndex;
    }

    /// <summary>
    /// Moves to the next item in the enumeration.
    /// </summary>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public bool MoveNext()
    {
        if (CurrentIndex == NotStartedIndex)
        {
            if (Slice.LongCount == 0) return false; // Nothing to enumerate
            else // There must be a first element
            {
                CurrentIndex++;
                return true;
            }
        }

        if (CurrentIndex == Slice.LongCount - 1) // Have reached the end of the slice
        {
            CurrentIndex = AlreadyFinishedIndex;
            return false;
        }

        CurrentIndex++;
        return true;
    }

    /// <summary>
    /// Resets the enumeration.
    /// </summary>
    public void Reset()
    {
        // Don't do anything for default instances so they don't change
        if (!IsDefault) CurrentIndex = NotStartedIndex;
    }

    /// <summary>
    /// Does nothing, as there are no resources to dispose of.
    /// </summary>
    public void Dispose() { }
}

/// <summary>
/// The core functionality for array slices.
/// </summary>
/// <typeparam name="T"></typeparam>
file readonly struct ArraySliceCore<T> : ISpecifiedEnumerable<ArraySliceEnumerator<T>, T>
{
    #region Properties And Indexers
    /// <summary>
    /// Determines if this slice is the degenerate <see langword="null"/>-wrapping value.
    /// </summary>
    public bool IsDefault => Array is null;

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="ElementAt(long)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public ref T ElementAt(Index index) => ref ElementAt((LongIndex)index);
#endif

    /// <inheritdoc cref="ElementAt(long)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public ref T ElementAt(LongIndex index) => ref Array[LongOffset + index.GetOffset(LongCount)];

    /// <inheritdoc cref="ElementAt(long)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public ref T ElementAt(int index) => ref Array[LongOffset + index];

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public ref T ElementAt(long index) => ref Array[LongOffset + index];

    /// <summary>
    /// Gets the array this instance is a slice for.
    /// </summary>
    [MaybeDefaultIfInstanceDefault]
    public T[] Array { get; }

    /// <summary>
    /// Gets the offset of this instance into the array as a 32-bit integer.
    /// </summary>
    public int Offset => (int)LongOffset;

    /// <summary>
    /// Gets the number of elements in this instance as a 64-bit integer.
    /// </summary>
    public int Count => (int)LongCount;

    /// <summary>
    /// Gets the offset of this instance into the array as a 64-bit integer.
    /// </summary>
    public long LongOffset { get; }

    /// <summary>
    /// Gets the number of elements in this instance as a 64-bit integer.
    /// </summary>
    public long LongCount { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Constructs a new slice representing the entire array passed in.
    /// </summary>
    /// <param name="array"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceCore(T[] array)
    {
        Array = array ?? throw new ArgumentNullException(nameof(array));
        LongOffset = 0;
        LongCount = array.LongLength;
    }

#if INDEX_RANGE_SUPPORTED
    /// <summary>
    /// Constructs a new slice that represents the slice of <paramref name="array"/> specified
    /// by <paramref name="range"/>.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="range"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="range"/> was out of bounds for <paramref name="array"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceCore(T[] array, Range range) : this(array, (LongRange)range) { }
#endif

    /// <summary>
    /// Constructs a new slice that represents the slice of the given array indicated by the
    /// given <see cref="LongRange"/>.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="range"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="range"/> was degenerate, or out of bounds for the array.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceCore(T[] array, LongRange range)
    {
        Array = array ?? throw new ArgumentNullException(nameof(array));

        try
        {
            (LongOffset, LongCount) = range.ThrowIfDegenerateArgument(nameof(range))
                                           .GetOffsetAndLength(array.LongLength);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentException("The range was out of bounds for the array.", nameof(range), ex);
        }
    }

    /// <summary>
    /// Constructs a new slice that represents the slice of <paramref name="array"/> specified
    /// by <paramref name="offset"/>.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="offset"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> was negative or out of bounds of <paramref name="array"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceCore(T[] array, long offset)
    {
        Array = array ?? throw new ArgumentNullException(nameof(array));

        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset cannot be negative.");
        if (offset > array.LongLength)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset is out of bounds of the segment.");
        }

        LongOffset = offset;
        LongCount = array.LongLength - LongOffset;
    }

    /// <summary>
    /// Constructs a new slice that represents the slice of array <paramref name="array"/> specified by
    /// <paramref name="offset"/> and <paramref name="count"/>.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> was negative or out of bounds
    /// of <paramref name="array"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceCore(T[] array, long offset, long count)
    {
        Array = array ?? throw new ArgumentNullException(nameof(array));

        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset cannot be negative.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be negative.");
        if (offset + count > array.LongLength)
        {
            throw new ArgumentOutOfRangeException(
                        "Offset and count specified a slice that was out of bounds of the segment.", default(Exception));
        }

        LongOffset = offset;
        LongCount = count;
    }
    #endregion

    #region Slicing
    #region Exception-Throwing
    /// <summary>
    /// Creates a slice equal to this instance truncated at the specified count.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Truncate</c> methods, this method will throw an <see cref="ArgumentOutOfRangeException"/> if the
    /// supplied count is out of range of the slice.
    /// </remarks>
    /// <param name="count">The count to truncate this instance to.</param>
    /// <returns>A slice equal to this instance truncated after <paramref name="count"/> elements.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The count was negative, or too long for this slice.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TruncateAt(long count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count was negative.");
        if (count > LongCount)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count was too large for the slice.");
        }
        return new(Array, LongOffset, count);
    }

    /// <summary>
    /// Creates a slice equal to this instance offset by the specific amount.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Skip</c> methods, this method will throw an <see cref="ArgumentOutOfRangeException"/> if the
    /// supplied offset is out of range of the slice.
    /// </remarks>
    /// <param name="offset">The offset to apply to this instance.</param>
    /// <returns>A slice equal to this instance offset by <paramref name="offset"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The offset was negative, or out of range for this slice.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Slice(long offset)
    {
        if (offset > LongCount)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset was out of range for the slice.");
        }
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset was negative.");

        return new(Array, LongOffset + offset, LongCount - offset);
    }

    /// <summary>
    /// Creates a slice consisting of the range of elements of this instance specified by the given
    /// <paramref name="offset"/> and <paramref name="count"/>.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Take</c> methods, this method will throw an <see cref="ArgumentOutOfRangeException"/> if the
    /// supplied offset or count is out of range of the slice.
    /// </remarks>
    /// <param name="offset">The offset at which to start taking elements.</param>
    /// <param name="count">The number of elements to take.</param>
    /// <returns>
    /// A slice consisting of <paramref name="count"/> elements of this instance, starting
    /// at <paramref name="offset"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> or <paramref name="count"/> was negative, or out of range for the slice.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Slice(long offset, long count)
    {
        if (offset > LongCount)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset was out of range for the slice.");
        }
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset was negative.");

        if (count > LongCount - offset)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count,
                                                  "Count is too large for the specified offset.");
        }
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Count was negative.");

        return new(Array, LongOffset + offset, count);
    }

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="Slice(LongRange)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Slice(Range range) => Slice((LongRange)range);
#endif

    /// <summary>
    /// Creates a slice consisting of the specified range of elements from this instance.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Take</c> methods, this method will throw an <see cref="ArgumentOutOfRangeException"/> if the
    /// supplied range is out of bounds of the slice.
    /// </remarks>
    /// <param name="range">The range indicating the elements to take.</param>
    /// <returns>
    /// A slice consisting of the  elements from this instance specified by <paramref name="range"/>
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="range"/> is out of bounds for this instance.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Slice(LongRange range)
    {
        try
        {
            var (offset, count) = range.ThrowIfDegenerateArgument(nameof(range)).GetOffsetAndLength(LongCount);
            return new(Array, LongOffset + offset, count);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentException("Range is out of bounds of the slice.", nameof(range), ex);
        }
    }
    #endregion

    #region Clamped
    /// <summary>
    /// Creates a slice consisting of the elements of this instance with the specified number of elements skipped.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Slice</c> methods, this method will clamp the count to valid values for the slice rather than
    /// throwing an exception.
    /// </remarks>
    /// <param name="count">The number of elements to skip.</param>
    /// <returns>A slice consisting of the elements of this instance with <paramref name="count"/> skipped.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Skip(long count)
    {
        count = count.Clamp(0, LongCount);
        return new(Array, LongOffset + count, LongCount - count);
    }

    /// <summary>
    /// Creates a slice consisting of the elements of this instance with the specified number of elements removed from
    /// the end.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Slice</c> methods, this method will clamp the count to valid values for the slice rather than
    /// throwing an exception.
    /// </remarks>
    /// <param name="count">The number of elements to remove from the end.</param>
    /// <returns>
    /// A slice consisting of the elements of this instance with <paramref name="count"/> removed from the end.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> SkipLast(long count)
    {
        count = count.Clamp(0, LongCount);
        return new(Array, 0, LongCount - count);
    }

    /// <summary>
    /// Creates a slice equivalent to this one with elements skipped until an element fails to satisfy the
    /// specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use to check elements.</param>
    /// <returns>
    /// A slice equivalent to this one with elements skipped until an element fails to
    /// satisfy <paramref name="predicate"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> SkipWhile(Func<T, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        long offset = 0;
        foreach (var e in this)
        {
            if (predicate(e)) offset++;
            else break;
        }
        return new(Array, offset, LongCount - offset);
    }

    /// <inheritdoc cref="SkipWhile(Func{T, long, bool})"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> SkipWhile(Func<T, int, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        int offset = 0;
        foreach (var e in this)
        {
            if (predicate(e, offset)) offset++;
            else break;
        }
        return new(Array, offset, LongCount - offset);
    }

    /// <summary>
    /// Creates a slice equivalent to this one with elements skipped until an element and index pair fails to satisfy
    /// the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use to check elements.</param>
    /// <returns>
    /// A slice equivalent to this one with elements skipped until an element and index pair fails to
    /// satisfy <paramref name="predicate"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> SkipWhile(Func<T, long, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        long offset = 0;
        foreach (var e in this)
        {
            if (predicate(e, offset)) offset++;
            else break;
        }
        return new(Array, offset, LongCount - offset);
    }

    /// <inheritdoc cref="TakeWhile(Func{T, long, bool})"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TakeWhile(Func<T, bool> predicate)
    {
        long count = 0;
        foreach (var e in this)
        {
            if (predicate(e)) count++;
            else break;
        }
        return new(Array, LongOffset, count);
    }

    /// <inheritdoc cref="TakeWhile(Func{T, long, bool})"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TakeWhile(Func<T, int, bool> predicate)
    {
        int count = 0;
        foreach (var e in this)
        {
            if (predicate(e, count)) count++;
            else break;
        }
        return new(Array, LongOffset, count);
    }

    /// <summary>
    /// Creates a new slice consisting of elements taken from the start of this instance while the given predicate
    /// is satisfied.
    /// </summary>
    /// <param name="predicate">A predicate to indicate when the resulting slice should end.</param>
    /// <returns>
    /// A new slice consisting of elements taked from the start of this instance while <paramref name="predicate"/>
    /// is satisfied.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TakeWhile(Func<T, long, bool> predicate)
    {
        long count = 0;
        foreach (var e in this)
        {
            if (predicate(e, count)) count++;
            else break;
        }
        return new(Array, LongOffset, count);
    }

    /// <inheritdoc cref="TakeLast(long)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TakeLast(int count) => TakeLast((long)count);

    /// <summary>
    /// Creates a new slice consisting of the specified number of elements taken from the end of this slice.
    /// </summary>
    /// <param name="count">The number of elements to take from the end of this slice.</param>
    /// <returns>A new slice consisting of the last <paramref name="count"/> elements of this slice.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> TakeLast(long count)
    {
        count = count.Clamp(0, LongCount);
        return new(Array, LongCount - count, count);
    }

    /// <summary>
    /// Creates a slice consisting of the specified number of elements starting at the beginning of this instance.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Truncate</c> methods, this method will clamp the specified count to the range of valid indices
    /// for this instance, rather than throwing an exception.
    /// </remarks>
    /// <param name="count">The number of elements to take.</param>
    /// <returns>A slice consisting of the first <paramref name="count"/> elements of the slice.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Take(long count)
    {
        count = count.Clamp(0, LongCount);
        return new(Array, LongOffset, count);
    }

    /// <summary>
    /// Creates a slice consisting of the series of elements specified by the given offset and length from
    /// this instance.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Slice</c> methods, this method will clamp the specified offset and count to the range of valid
    /// values for this instance, rather than throwing an exception.
    /// </remarks>
    /// <param name="offset">The offset to start taking elements at.</param>
    /// <param name="count">The number of elements to take.</param>
    /// <returns>
    /// A slice consisting of the series of <paramref name="count"/> elements starting at <paramref name="offset"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Take(long offset, long count)
    {
        offset = offset.Clamp(0, LongCount);
        count = count.Clamp(0, LongCount - offset);
        return new(Array, LongOffset + offset, count);
    }

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="Take(LongRange)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Take(Range range) => Take((LongRange)range);
#endif

    /// <summary>
    /// Creates a slice consisting of the series of elements specified by the given range from this instance.
    /// </summary>
    /// <remarks>
    /// Unlike the <c>Slice</c> methods, this method will clamp the specified range to the range of valid values
    /// for this instance, rather than throwing an exception.
    /// </remarks>
    /// <param name="range">The range indicating the elements to take.</param>
    /// <returns>A slice consisting of the series of elements specified by <paramref name="range"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ArraySliceCore<T> Take(LongRange range)
    {
        var (offset, count) = range.GetClampedOffsetAndLength(LongCount);
        return new(Array, LongOffset + offset, count);
    }
    #endregion
    #endregion

    #region GetEnumerator
    /// <summary>
    /// Gets an object that can be used to traverse this instance.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySliceEnumerator<T> GetEnumerator() => new(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<T> GetEnumeratorObject() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumeratorObject();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumeratorObject();
    #endregion

    #region ICollection and IList Implementations
    /// <summary>
    /// Determines whether or not this slice contains the element passed in.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public bool Contains(T item) => IndexOf(item) != -1;

    /// <summary>
    /// Gets the index of the element passed in.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public int IndexOf(T item) => ((IList<T>)Array).IndexOf(item) switch
    {
        -1 => -1,
        var i => i >= Offset && i < Offset + Count ? i - Offset : -1,
    };

    /// <summary>
    /// Copies this slice to the specified array, starting at the specified index.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    public void CopyTo(T[] array, int arrayIndex)
    {
        Buffer.BlockCopy(Array, Offset, array, arrayIndex, Count);
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this slice is equal to another.
    /// </summary>
    /// <param name="other">Another slice to compare with.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(in ArraySliceCore<T> other)
        => IsDefault
            ? other.IsDefault
            : !other.IsDefault && Array == other.Array
                               && LongOffset == other.LongOffset
                               && LongCount == other.LongCount;

    /// <summary>
    /// Gets a hash code representing this slice.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public new int GetHashCode() => HashCode.Combine(Array, LongOffset, LongCount);
    #endregion

    #region Conversions
    /// <summary>
    /// Creates a new slice of this type from the given slice of a child type, using array covariance to avoid
    /// allocating another array.
    /// </summary>
    /// <typeparam name="TChild">The type of the elements of the slice to be converted to this slice type.</typeparam>
    /// <param name="slice">The array slice to convert.</param>
    /// <returns>
    /// A new slice equivalent to <paramref name="slice"/> typed as <typeparamref name="T"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotDefaultIfNotDefault(nameof(slice))]
    public static ArraySliceCore<T> FromChild<TChild>(in ArraySliceCore<TChild> slice) where TChild : class, T
        => new(slice.Array, slice.LongOffset, slice.LongCount);

    /// <summary>
    /// Implicitly converts an array to a slice wrapping the entire array.
    /// </summary>
    /// <remarks>
    /// <see langword="null"/> values will be mapped to the default.
    /// </remarks>
    /// <param name="array"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: MaybeDefaultIfDefault(nameof(array))]
    public static implicit operator ArraySliceCore<T>(T[]? array) => array is null ? default : new(array);

    /// <summary>
    /// Implicitly converts a <see cref="ReadOnlyArray{T}"/> to a slice representing the entire array.
    /// </summary>
    /// <remarks>Default values will be mapped to the default.</remarks>
    /// <param name="array"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: MaybeDefaultIfDefault(nameof(array))]
    public static implicit operator ArraySliceCore<T>(ReadOnlyArray<T> array)
        => array.IsDefault ? default : new(array._array);

    /// <summary>
    /// Implicitly converts a .NET <see cref="ArraySegment{T}"/> to a slice.
    /// </summary>
    /// <remarks>Default values will be mapped to the default.</remarks>
    /// <param name="segment"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ArraySliceCore<T>(ArraySegment<T> segment)
        => segment.Array is null ? default : new(segment.Array, segment.Offset, segment.Count);

    /// <summary>
    /// Implicitly converts a slice to a <see cref="Span{T}"/> spanning it.
    /// </summary>
    /// <param name="slice"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Span<T>(in ArraySliceCore<T> slice) => new(slice.Array, slice.Offset, slice.Count);

    /// <summary>
    /// Implicitly converts a slice to a <see cref="ReadOnlySpan{T}"/> spanning it.
    /// </summary>
    /// <param name="slice"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlySpan<T>(in ArraySliceCore<T> slice)
        => new(slice.Array, slice.Offset, slice.Count);

    /// <summary>
    /// Implicitly converts a slice to an equivalent <see cref="ArraySegment{T}"/>.
    /// </summary>
    /// <param name="slice"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ArraySegment<T>(in ArraySliceCore<T> slice)
        => new(slice.Array, slice.Offset, slice.Count);
    #endregion
}
#endregion
