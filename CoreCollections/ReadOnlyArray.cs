using Rem.Core.Attributes;
using Rem.Core.Collections.Enumeration;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Static functionality relating to the generic <see cref="ReadOnlyArray{T}"/> struct.
/// </summary>
public static class ReadOnlyArray
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

    #region ReadOnlyArray<T> Extensions
    #region LINQ
    #region Aggregate
    /// <summary>
    /// Applies an accumulator function over the current <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <inheritdoc cref="Aggregate{TSource, TAccumulate, TResult}(ReadOnlyArray{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/>
    public static T Aggregate<T>([NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, T, T> func)
        => array.ThrowIfArgDefault(nameof(array))._array.Aggregate(func);

    /// <summary>
    /// Applies an accumulator function over the current <see cref="ReadOnlyArray{T}"/>.
    /// The specified seed value is used as the initial accumulator value.
    /// </summary>
    /// <inheritdoc cref="Aggregate{TSource, TAccumulate, TResult}(ReadOnlyArray{TSource}, TAccumulate, Func{TAccumulate, TSource, TAccumulate}, Func{TAccumulate, TResult})"/>
    public static TAccumulate Aggregate<TSource, TAccumulate>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> array,
        TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        => array.ThrowIfArgDefault(nameof(array))._array.Aggregate(seed, func);

    /// <summary>
    /// Applies an accumulator function over the current <see cref="ReadOnlyArray{T}"/>.
    /// The specified seed value is used as the initial accumulator value, and the specified function is used to
    /// select the result value.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TAccumulate"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="array"></param>
    /// <param name="seed"></param>
    /// <param name="func"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> was <see langword="null"/>.</exception>
    public static TResult Aggregate<TSource, TAccumulate, TResult>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> array,
        TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        => array.ThrowIfArgDefault(nameof(array))._array.Aggregate(seed, func, resultSelector);
    #endregion

    #region All
    /// <summary>
    /// Determines if all instances of the current <see cref="ReadOnlyArray{T}"/> satisfy the supplied predicate.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="array"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool All<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> array,
                                    Func<TSource, bool> predicate)
        => array.ThrowIfArgDefault(nameof(array))._array.All(predicate);
    #endregion

    #region Any
    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> contains any elements.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool Any<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> array)
        => array.ThrowIfArgDefault(nameof(array))._array.LongLength != 0;

    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> contains any elements that satisfy the
    /// given predicate.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="array"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool Any<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> array,
                                    Func<TSource, bool> predicate)
        => array.ThrowIfArgDefault(nameof(array))._array.Any(predicate);
    #endregion

#if NET471_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP || NET5_0_OR_GREATER
    #region Append
    /// <inheritdoc cref="Enumerable.Append{TSource}(IEnumerable{TSource}, TSource)"/>
    public static IEnumerable<TSource> Append<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                       TSource value)
        => source._array.Append(value);
    #endregion
#endif

    #region Average
    /// <inheritdoc cref="Enumerable.Average(IEnumerable{decimal})"/>
    public static decimal Average([NonDefaultableStruct] this ReadOnlyArray<decimal> source)
        => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{decimal?})"/>
    public static decimal? Average([NonDefaultableStruct] this ReadOnlyArray<decimal?> source)
        => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{double})"/>
    public static double Average([NonDefaultableStruct] this ReadOnlyArray<double> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{double?})"/>
    public static double? Average([NonDefaultableStruct] this ReadOnlyArray<double?> source)
        => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{float})"/>
    public static float Average([NonDefaultableStruct] this ReadOnlyArray<float> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{float?})"/>
    public static float? Average([NonDefaultableStruct] this ReadOnlyArray<float?> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{int})"/>
    public static double Average([NonDefaultableStruct] this ReadOnlyArray<int> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{int?})"/>
    public static double? Average([NonDefaultableStruct] this ReadOnlyArray<int?> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{long})"/>
    public static double Average([NonDefaultableStruct] this ReadOnlyArray<long> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average(IEnumerable{long?})"/>
    public static double? Average([NonDefaultableStruct] this ReadOnlyArray<long?> source) => source._array.Average();

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, decimal})"/>
    public static decimal Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                           Func<TSource, decimal> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, decimal?})"/>
    public static decimal? Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                            Func<TSource, decimal?> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, double})"/>
    public static double Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                          Func<TSource, double> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, double?})"/>
    public static double? Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                           Func<TSource, double?> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, float})"/>
    public static float Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                         Func<TSource, float> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, float?})"/>
    public static float? Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                          Func<TSource, float?> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, int})"/>
    public static double Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                          Func<TSource, int> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, int?})"/>
    public static double? Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, int?> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, long})"/>
    public static double Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                          Func<TSource, long> selector)
        => source._array.Average(selector);

    /// <inheritdoc cref="Enumerable.Average{TSource}(IEnumerable{TSource}, Func{TSource, long?})"/>
    public static double? Average<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                           Func<TSource, long?> selector)
        => source._array.Average(selector);
    #endregion

#if NET6_0_OR_GREATER
    #region Chunk
    // Included as instance methods
    #endregion
#endif

    #region Concat
    /// <inheritdoc cref="Concat{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild})"/>
    public static IEnumerable<TParent> Concat<TParent, TChild>(this IEnumerable<TParent> source,
                                                               [NonDefaultableStruct] ReadOnlyArray<TChild> other)
    where TChild : TParent
        => source.Concat(other.ToParent<TChild, TParent>());

    /// <summary>
    /// Creates a new enumerable sequence-equal to the current sequence to a <see cref="ReadOnlyArray{T}"/> of a
    /// child type.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current sequence.</typeparam>
    /// <typeparam name="TChild">
    /// A child type of <typeparamref name="TParent"/> that is the element type of the <see cref="ReadOnlyArray{T}"/>
    /// forming the other sequence.
    /// </typeparam>
    /// <param name="source">The current sequence.</param>
    /// <param name="other">The other sequence.</param>
    /// <returns>
    /// A new enumerable sequence-equal to <paramref name="source"/> concatenated to <paramref name="other"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// One of the parameters was <see langword="null"/> or the default.
    /// </exception>
    public static IEnumerable<TParent> Concat<TParent, TChild>(
        [NonDefaultableStruct] this ReadOnlyArray<TParent> source,
        [NonDefaultableStruct] ReadOnlyArray<TChild> other)
    where TChild : TParent
        => source._array.Concat(other.ToParent<TChild, TParent>());

    /// <inheritdoc cref="Enumerable.Concat{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Concat<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                       IEnumerable<TSource> other)
        => source._array.Concat(other);
    #endregion

    #region Contains
    /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource)"/>
    public static bool Contains<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source, TSource value)
        => source._array.Contains(value);

    /// <inheritdoc cref="Enumerable.Contains{TSource}(IEnumerable{TSource}, TSource, IEqualityComparer{TSource}?)"/>
    public static bool Contains<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                         TSource value, IEqualityComparer<TSource>? comparer)
        => source._array.Contains(value, comparer);
    #endregion

    #region Count
    /// <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>
    public static int Count<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source) => source.Length;

    /// <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/>
    public static int Count<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, bool> predicate)
        => source._array.Count(predicate);
    #endregion

    #region DefaultIfEmpty
    /// <inheritdoc cref="Enumerable.DefaultIfEmpty{TSource}(IEnumerable{TSource})"/>
    public static IEnumerable<TSource?> DefaultIfEmpty<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.DefaultIfEmpty();

    /// <inheritdoc cref="Enumerable.DefaultIfEmpty{TSource}(IEnumerable{TSource}, TSource)"/>
    public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, TSource defaultValue)
        => source._array.DefaultIfEmpty(defaultValue);
    #endregion

    #region Distinct
    /// <inheritdoc cref="Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Distinct<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Distinct();

    /// <inheritdoc cref="Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Distinct<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                         IEqualityComparer<TSource>? comparer)
        => source._array.Distinct(comparer);
    #endregion

#if NET6_0_OR_GREATER
    #region DistinctBy
    /// <inheritdoc cref="Enumerable.DistinctBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, TKey> keySelector)
        => source._array.DistinctBy(keySelector);

    /// <inheritdoc cref="Enumerable.DistinctBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        => source._array.DistinctBy(keySelector, comparer);
    #endregion
#endif

    #region ElementAt
    /// <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
    public static TSource ElementAt<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source, int index)
        => source._array.ElementAt(index);

    /// <summary>
    /// Gets the element at the specified index of the current sequence.
    /// </summary>
    /// <typeparam name="TSource">The element type of the current sequence.</typeparam>
    /// <param name="source">The current sequence.</param>
    /// <param name="index">The index to get from the current sequence.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="StructArgumentDefaultException">The current sequence was the default.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The specified index was out of range of the current sequence.
    /// </exception>
    public static TSource ElementAt<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                             LongIndex index)
    {
        if (source.IsDefault) throw new StructArgumentDefaultException(nameof(source));
        return source[index];
    }

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, Index)"/>
    public static TSource ElementAt<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source, Index index)
        => source._array.ElementAt(index);
#endif
    #endregion

    #region ElementAtOrDefault
    /// <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
    [return: MaybeDefault]
    public static TSource? ElementAtOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                       int index)
        => source._array.ElementAtOrDefault(index);

    /// <summary>
    /// Gets the element at the specified index into the current sequence, or the default value of type
    /// <typeparamref name="TSource"/> if the specified index is out of range.
    /// </summary>
    /// <typeparam name="TSource">The element type of the current sequence.</typeparam>
    /// <param name="source">The current sequence.</param>
    /// <param name="index">The index into the collection to get the element at.</param>
    /// <returns>
    /// The element at index <paramref name="index"/> into collection <paramref name="source"/>, or
    /// <see langword="default"/> if the specified index is out of range.
    /// </returns>
    /// <exception cref="StructArgumentDefaultException">The current sequence is the default.</exception>
    public static TSource? ElementAtOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                       LongIndex index)
    {
        if (source.IsDefault) throw new StructArgumentDefaultException(nameof(source));
        return index.TryGetUnclampedOffset(source.LongLength, out var offset)
                ? source[offset]
                : default;
    }

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, Index)"/>
    [return: MaybeDefault]
    public static TSource? ElementAtOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                       Index index)
        => source._array.ElementAtOrDefault(index);
#endif
    #endregion

    #region Except
    /// <inheritdoc cref="Except{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Except<TParent, TChild>(
            this IEnumerable<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second)
    where TChild : TParent
        => first.Except(second.ToParent<TChild, TParent>());

    /// <inheritdoc cref="Except{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Except<TParent, TChild>(
            this IEnumerable<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second,
            IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.Except(second.ToParent<TChild, TParent>(), comparer);

    /// <inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Except<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TSource> second)
        => first._array.Except(second);

    /// <inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static IEnumerable<TSource> Except<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource>? comparer)
        => first._array.Except(second, comparer);

    /// <inheritdoc cref="Except{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Except<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second)
    where TChild : TParent
        => first._array.Except(second.ToParent<TChild, TParent>());

    /// <summary>
    /// Computes the set-difference of the current sequence and another sequence of a child type.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current sequence.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the second sequence.
    /// </typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">A sequence of elements to remove.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare elements of type <typeparamref name="TParent"/>, or
    /// <see langword="null"/> to use the default equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns>
    /// A new sequence equivalent to <paramref name="first"/> with the elements of <paramref name="second"/> removed.
    /// </returns>
    public static IEnumerable<TParent> Except<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second,
            IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first._array.Except(second.ToParent<TChild, TParent>(), comparer);
    #endregion

#if NET6_0_OR_GREATER
    #region ExceptBy
    /// <inheritdoc cref="ExceptBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> ExceptBy<TSource, TParentKey, TChildKey>(
            this IEnumerable<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second, Func<TSource, TParentKey> keySelector)
    where TChildKey : TParentKey
        => first.ExceptBy(second.ToParent<TChildKey, TParentKey>(), keySelector);

    /// <inheritdoc cref="Enumerable.ExceptBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey})"/>
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TKey> second, Func<TSource, TKey> keySelector)
        => first._array.ExceptBy(second, keySelector);

    /// <inheritdoc cref="ExceptBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> ExceptBy<TSource, TParentKey, TChildKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second, Func<TSource, TParentKey> keySelector)
    where TChildKey : TParentKey
        => first._array.ExceptBy(second.ToParent<TChildKey, TParentKey>(), keySelector);

    /// <inheritdoc cref="ExceptBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> ExceptBy<TSource, TParentKey, TChildKey>(
            this IEnumerable<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector, IEqualityComparer<TParentKey>? comparer)
    where TChildKey : TParentKey
        => first.ExceptBy(second.ToParent<TChildKey, TParentKey>(), keySelector, comparer);

    /// <inheritdoc cref="Enumerable.ExceptBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TSource> ExceptBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        => first._array.ExceptBy(second, keySelector, comparer);

    /// <summary>
    /// Calculates the set-difference of the current sequence and another sequence of keys by mapping each element to
    /// a key.
    /// </summary>
    /// <typeparam name="TSource">The element type of the current sequence.</typeparam>
    /// <typeparam name="TParentKey">The type of the keys mapped to.</typeparam>
    /// <typeparam name="TChildKey">
    /// A subtype of <typeparamref name="TParentKey"/> that is the element type of the key sequence.
    /// </typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">Another sequence of keys to remove from the current sequence.</param>
    /// <param name="keySelector">A function to use to map elements of <paramref name="first"/> to keys.</param>
    /// <param name="comparer">
    /// An equality comparer to use to compare keys, or <see langword="null"/> to use the default equality comparer
    /// for type <typeparamref name="TParentKey"/>.
    /// </param>
    /// <returns>
    /// The set-difference between <paramref name="first"/> after transformation by <paramref name="keySelector"/>
    /// and <paramref name="second"/>.
    /// </returns>
    public static IEnumerable<TSource> ExceptBy<TSource, TParentKey, TChildKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector, IEqualityComparer<TParentKey>? comparer)
    where TChildKey : TParentKey
        => first._array.ExceptBy(second.ToParent<TChildKey, TParentKey>(), keySelector, comparer);
    #endregion
#endif

    #region First
    /// <inheritdoc cref="Enumerable.First{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    public static TSource First<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                         Func<TSource, bool> predicate)
        => source._array.First(predicate);

    /// <inheritdoc cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
    public static TSource First<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.First();
    #endregion

    #region FirstOrDefault
    /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static TSource? FirstOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.FirstOrDefault();

    /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    public static TSource? FirstOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                   Func<TSource, bool> predicate)
        => source._array.FirstOrDefault(predicate);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static TSource FirstOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                  Func<TSource, bool> predicate, TSource defaultValue)
        => source._array.FirstOrDefault(predicate, defaultValue);

    /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static TSource FirstOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                  TSource defaultValue)
        => source._array.FirstOrDefault(defaultValue);
#endif
    #endregion

    #region GroupBy
    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TElement, TResult}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, Func{TKey, IEnumerable{TElement}, TResult})"/>
    public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        => source._array.GroupBy(keySelector, elementSelector, resultSelector);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TElement, TResult}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, Func{TKey, IEnumerable{TElement}, TResult})"/>
    public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        => source._array.GroupBy(keySelector, elementSelector, resultSelector, comparer);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TElement}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement})"/>
    public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        => source._array.GroupBy(keySelector, elementSelector);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TElement}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        => source._array.GroupBy(keySelector, elementSelector, comparer);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TResult}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TKey, IEnumerable{TSource}, TResult})"/>
    public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        => source._array.GroupBy(keySelector, resultSelector);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey, TResult}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TKey, IEnumerable{TSource}, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        => source._array.GroupBy(keySelector, resultSelector, comparer);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, TKey> keySelector)
        => source._array.GroupBy(keySelector);

    /// <inheritdoc cref="Enumerable.GroupBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        => source._array.GroupBy(keySelector, comparer);
    #endregion

    #region GroupJoin
    /// <inheritdoc cref="Enumerable.GroupJoin{TOuter, TInner, TKey, TResult}(IEnumerable{TOuter}, IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, IEnumerable{TInner}, TResult})"/>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        => outer._array.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);

    /// <inheritdoc cref="Enumerable.GroupJoin{TOuter, TInner, TKey, TResult}(IEnumerable{TOuter}, IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, IEnumerable{TInner}, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
        => outer._array.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    /// <inheritdoc cref="GroupJoin{TOuter, TInnerParent, TInnerChild, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TInnerChild}, Func{TOuter, TKey}, Func{TInnerParent, TKey}, Func{TOuter, IEnumerable{TInnerParent}, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInnerParent, TInnerChild, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TInnerChild> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInnerParent, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInnerParent>, TResult> resultSelector)
    where TInnerChild : TInnerParent
        => outer.GroupJoin(inner.ToParent<TInnerChild, TInnerParent>(),
                           outerKeySelector, innerKeySelector, resultSelector);

    /// <inheritdoc cref="GroupJoin{TOuter, TInnerParent, TInnerChild, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TInnerChild}, Func{TOuter, TKey}, Func{TInnerParent, TKey}, Func{TOuter, IEnumerable{TInnerParent}, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInnerParent, TInnerChild, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TInnerChild> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInnerParent, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInnerParent>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
    where TInnerChild : TInnerParent
        => outer.GroupJoin(inner.ToParent<TInnerChild, TInnerParent>(),
                           outerKeySelector, innerKeySelector, resultSelector, comparer);

    /// <inheritdoc cref="GroupJoin{TOuter, TInnerParent, TInnerChild, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TInnerChild}, Func{TOuter, TKey}, Func{TInnerParent, TKey}, Func{TOuter, IEnumerable{TInnerParent}, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInnerParent, TInnerChild, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TInnerChild> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInnerParent, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInnerParent>, TResult> resultSelector)
    where TInnerChild : TInnerParent
        => outer._array.GroupJoin(inner.ToParent<TInnerChild, TInnerParent>(),
                                  outerKeySelector, innerKeySelector, resultSelector);

    /// <summary>
    /// Correlates the elements of the current sequence and another based on their keys, and groups the results.
    /// </summary>
    /// <typeparam name="TOuter">The element type of the current sequence.</typeparam>
    /// <typeparam name="TInnerParent">
    /// The type of inner elements of the join.
    /// </typeparam>
    /// <typeparam name="TInnerChild">
    /// A subtype of <typeparamref name="TInnerParent"/> that is the element type of the inner sequence.
    /// </typeparam>
    /// <typeparam name="TKey">The type of keys to use to correlate the elements.</typeparam>
    /// <typeparam name="TResult">The type of the grouped results.</typeparam>
    /// <param name="outer">The current sequence.</param>
    /// <param name="inner">The other sequence.</param>
    /// <param name="outerKeySelector">
    /// The selector to use to map elements of the current sequence to keys.
    /// </param>
    /// <param name="innerKeySelector">
    /// The selector to use to map elements of <paramref name="inner"/> to keys.
    /// </param>
    /// <param name="resultSelector">The selector to use to group the results.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare keys, or <see langword="null"/> to use the default
    /// equality comparer for type <typeparamref name="TKey"/>.
    /// </param>
    /// <returns></returns>
    public static IEnumerable<TResult> GroupJoin<TOuter, TInnerParent, TInnerChild, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TInnerChild> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInnerParent, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInnerParent>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer)
    where TInnerChild : TInnerParent
        => outer._array.GroupJoin(inner.ToParent<TInnerChild, TInnerParent>(),
                                  outerKeySelector, innerKeySelector, resultSelector, comparer);
    #endregion

    #region Intersect
    /// <inheritdoc cref="Intersect{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Intersect<TParent, TChild>(this IEnumerable<TParent> first,
                                                                  [NonDefaultableStruct] ReadOnlyArray<TChild> second)
    where TChild : TParent
        => first.Intersect(second.ToParent<TChild, TParent>());

    /// <inheritdoc cref="Intersect{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Intersect<TParent, TChild>(this IEnumerable<TParent> first,
                                                                  [NonDefaultableStruct] ReadOnlyArray<TChild> second,
                                                                  IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first.Intersect(second.ToParent<TChild, TParent>(), comparer);

    /// <inheritdoc cref="Enumerable.Intersect{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Intersect<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> first,
                                                          IEnumerable<TSource> second)
        => first._array.Intersect(second);

    /// <inheritdoc cref="Enumerable.Intersect{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static IEnumerable<TSource> Intersect<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> first,
                                                          IEnumerable<TSource> second,
                                                          IEqualityComparer<TSource>? comparer)
        => first._array.Intersect(second, comparer);

    /// <inheritdoc cref="Intersect{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Intersect<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second)
    where TChild : TParent
        => first._array.Intersect(second.ToParent<TChild, TParent>());

    /// <summary>
    /// Computes the intersection between the current sequence and another.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current sequence.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">Another sequence to compute the intersection with.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare elements of the sequences, or <see langword="null"/>
    /// to use the default equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns>The intersection between <paramref name="first"/> and <paramref name="second"/>.</returns>
    public static IEnumerable<TParent> Intersect<TParent, TChild>(
        [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
        [NonDefaultableStruct] ReadOnlyArray<TChild> second, IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => first._array.Intersect(second.ToParent<TChild, TParent>(), comparer);
    #endregion

#if NET6_0_OR_GREATER
    #region IntersectBy
    /// <inheritdoc cref="IntersectBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> IntersectBy<TSource, TParentKey, TChildKey>(
            this IEnumerable<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector)
    where TChildKey : TParentKey
        => first.IntersectBy(second.ToParent<TChildKey, TParentKey>(), keySelector);

    /// <inheritdoc cref="Enumerable.IntersectBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey})"/>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector)
        => first._array.IntersectBy(second, keySelector);

    /// <inheritdoc cref="IntersectBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> IntersectBy<TSource, TParentKey, TChildKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector)
    where TChildKey : TParentKey
        => first._array.IntersectBy(second.ToParent<TChildKey, TParentKey>(), keySelector);

    /// <inheritdoc cref="IntersectBy{TSource, TParentKey, TChildKey}(ReadOnlyArray{TSource}, ReadOnlyArray{TChildKey}, Func{TSource, TParentKey}, IEqualityComparer{TParentKey}?)"/>
    public static IEnumerable<TSource> IntersectBy<TSource, TParentKey, TChildKey>(
            this IEnumerable<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector,
            IEqualityComparer<TParentKey>? keyComparer)
    where TChildKey : TParentKey
        => first.IntersectBy(second.ToParent<TChildKey, TParentKey>(), keySelector, keyComparer);

    /// <inheritdoc cref="Enumerable.IntersectBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TKey}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? keyComparer)
        => first._array.IntersectBy(second, keySelector, keyComparer);

    /// <summary>
    /// Computes the intersection of the current sequence and another sequence of a different type using a key
    /// selector function.
    /// </summary>
    /// <typeparam name="TSource">The element type of the current sequence.</typeparam>
    /// <typeparam name="TParentKey">The return type of the key selector function.</typeparam>
    /// <typeparam name="TChildKey">
    /// A subtype of <typeparamref name="TParentKey"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">The other sequence to compute the intersection with.</param>
    /// <param name="keySelector">
    /// A key selector to use to map the elements of <paramref name="first"/> to elements of
    /// type <typeparamref name="TParentKey"/>.
    /// </param>
    /// <param name="keyComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare keys, or <see langword="null"/> to use the default
    /// equality comparer for type <typeparamref name="TParentKey"/>.
    /// </param>
    /// <returns></returns>
    public static IEnumerable<TSource> IntersectBy<TSource, TParentKey, TChildKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            [NonDefaultableStruct] ReadOnlyArray<TChildKey> second,
            Func<TSource, TParentKey> keySelector,
            IEqualityComparer<TParentKey>? keyComparer)
    where TChildKey : TParentKey
        => first._array.IntersectBy(second.ToParent<TChildKey, TParentKey>(), keySelector, keyComparer);
    #endregion
#endif

    #region Join
    /// <inheritdoc cref="Join{TOuter, TParentInner, TChildInner, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TChildInner}, Func{TOuter, TKey}, Func{TParentInner, TKey}, Func{TOuter, TParentInner, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> Join<TOuter, TParentInner, TChildInner, TKey, TResult>(
            this IEnumerable<TOuter> outer, [NonDefaultableStruct] ReadOnlyArray<TChildInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TParentInner, TKey> innerKeySelector,
            Func<TOuter, TParentInner, TResult> resultSelector)
    where TChildInner : TParentInner
        => outer.Join(inner.ToParent<TChildInner, TParentInner>(), outerKeySelector, innerKeySelector, resultSelector);

    /// <inheritdoc cref="Join{TOuter, TParentInner, TChildInner, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TChildInner}, Func{TOuter, TKey}, Func{TParentInner, TKey}, Func{TOuter, TParentInner, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> Join<TOuter, TParentInner, TChildInner, TKey, TResult>(
            this IEnumerable<TOuter> outer, [NonDefaultableStruct] ReadOnlyArray<TChildInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TParentInner, TKey> innerKeySelector,
            Func<TOuter, TParentInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
    where TChildInner : TParentInner
        => outer.Join(inner.ToParent<TChildInner, TParentInner>(),
                      outerKeySelector, innerKeySelector, resultSelector, comparer);

    /// <inheritdoc cref="Enumerable.Join{TOuter, TInner, TKey, TResult}(IEnumerable{TOuter}, IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, TInner, TResult})"/>
    public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer, IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        => outer._array.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

    /// <inheritdoc cref="Enumerable.Join{TOuter, TInner, TKey, TResult}(IEnumerable{TOuter}, IEnumerable{TInner}, Func{TOuter, TKey}, Func{TInner, TKey}, Func{TOuter, TInner, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer, IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
        => outer._array.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    /// <inheritdoc cref="Join{TOuter, TParentInner, TChildInner, TKey, TResult}(ReadOnlyArray{TOuter}, ReadOnlyArray{TChildInner}, Func{TOuter, TKey}, Func{TParentInner, TKey}, Func{TOuter, TParentInner, TResult}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TResult> Join<TOuter, TParentInner, TChildInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TChildInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TParentInner, TKey> innerKeySelector,
            Func<TOuter, TParentInner, TResult> resultSelector)
    where TChildInner : TParentInner
        => outer._array.Join(inner.ToParent<TChildInner, TParentInner>(),
                             outerKeySelector, innerKeySelector, resultSelector);

    /// <summary>
    /// Joins the current sequence with another sequence of a different type by using key selectors to pair the
    /// elements and a result selector to generate results from the corresponding pairs.
    /// </summary>
    /// <typeparam name="TOuter">The element type of the current sequence.</typeparam>
    /// <typeparam name="TParentInner">The parameter type of the inner key selector.</typeparam>
    /// <typeparam name="TChildInner">
    /// A subtype of <typeparamref name="TParentInner"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <typeparam name="TKey">The type of keys produced by the key selectors.</typeparam>
    /// <typeparam name="TResult">The type of results produced by the result selector.</typeparam>
    /// <param name="outer">The current sequence.</param>
    /// <param name="inner">The sequence to join with.</param>
    /// <param name="outerKeySelector">The key selector for the elements of <paramref name="outer"/>.</param>
    /// <param name="innerKeySelector">The key selector for the elements of <paramref name="inner"/>.</param>
    /// <param name="resultSelector">The result selector.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare keys, or <see langword="null"/> to use the default
    /// equality comparer for type <typeparamref name="TKey"/>.
    /// </param>
    /// <returns>The join between <paramref name="outer"/> and <paramref name="inner"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// One of the sequences or selector functions was <see langword="null"/>.
    /// </exception>
    public static IEnumerable<TResult> Join<TOuter, TParentInner, TChildInner, TKey, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TOuter> outer,
            [NonDefaultableStruct] ReadOnlyArray<TChildInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TParentInner, TKey> innerKeySelector,
            Func<TOuter, TParentInner, TResult> resultSelector, IEqualityComparer<TKey>? comparer)
    where TChildInner : TParentInner
        => outer._array.Join(inner.ToParent<TChildInner, TParentInner>(),
                             outerKeySelector, innerKeySelector,
                             resultSelector, comparer);
    #endregion

    #region Last
    /// <inheritdoc cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
    public static TSource Last<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, bool> predicate)
        => source._array.Last(predicate);

    /// <inheritdoc cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
    public static TSource Last<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Last();
    #endregion

    #region LastOrDefault
    /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})" />
    [return: MaybeNull, MaybeDefault]
    public static TSource LastOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.LastOrDefault();

    /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource LastOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                 Func<TSource, bool> predicate)
        => source._array.LastOrDefault(predicate);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, TSource)"/>
    public static TSource LastOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                 TSource defaultValue)
        => source._array.LastOrDefault(defaultValue);

    /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool}, TSource)"/>
    public static TSource LastOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                 Func<TSource, bool> predicate, TSource defaultValue)
        => source._array.LastOrDefault(predicate, defaultValue);
#endif
    #endregion

    #region LongCount
    /// <inheritdoc cref="Enumerable.LongCount{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    public static long LongCount<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                          Func<TSource, bool> predicate)
        => source._array.LongCount(predicate);

    /// <inheritdoc cref="Enumerable.LongCount{TSource}(IEnumerable{TSource})"/>
    public static long LongCount<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.LongLength;
    #endregion

    #region Max
    /// <inheritdoc cref="Enumerable.Max(IEnumerable{decimal})"/>
    public static decimal Max([NonDefaultableStruct] this ReadOnlyArray<decimal> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{decimal?})"/>
    public static decimal? Max([NonDefaultableStruct] this ReadOnlyArray<decimal?> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{double})"/>
    public static double Max([NonDefaultableStruct] this ReadOnlyArray<double> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{double?})"/>
    public static double? Max([NonDefaultableStruct] this ReadOnlyArray<double?> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{float})"/>
    public static float Max([NonDefaultableStruct] this ReadOnlyArray<float> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{float?})"/>
    public static float? Max([NonDefaultableStruct] this ReadOnlyArray<float?> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{int})"/>
    public static int Max([NonDefaultableStruct] this ReadOnlyArray<int> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{int?})"/>
    public static int? Max([NonDefaultableStruct] this ReadOnlyArray<int?> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{long})"/>
    public static long Max([NonDefaultableStruct] this ReadOnlyArray<long> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max(IEnumerable{long?})"/>
    public static long? Max([NonDefaultableStruct] this ReadOnlyArray<long?> source) => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource})"/>
    public static TSource? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Max();

    /// <inheritdoc cref="Enumerable.Max{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult}})"/>
    public static TResult? Max<TSource, TResult>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                 Func<TSource, TResult> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, decimal})"/>
    public static decimal Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, decimal?})"/>
    public static decimal? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal?> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, double})"/>
    public static double Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, double> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, double?})"/>
    public static double? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                       Func<TSource, double?> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, float})"/>
    public static float Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, float> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, float?})"/>
    public static float? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, float?> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, int})"/>
    public static int Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                   Func<TSource, int> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, int?})"/>
    public static int? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, int?> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, long})"/>
    public static long Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, long> selector)
        => source._array.Max(selector);

    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, Func{TSource, long?})"/>
    public static long? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, long?> selector)
        => source._array.Max(selector);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource}, IComparer{TSource}?)"/>
    public static TSource? Max<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        IComparer<TSource>? comparer)
        => source._array.Max(comparer);
#endif
    #endregion

#if NET6_0_OR_GREATER
    #region MaxBy
    /// <inheritdoc cref="Enumerable.MaxBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource MaxBy<TSource, TKey>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                               Func<TSource, TKey> keySelector)
        => source._array.MaxBy(keySelector);

    /// <inheritdoc cref="Enumerable.MaxBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource MaxBy<TSource, TKey>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                               Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        => source._array.MaxBy(keySelector, comparer);
    #endregion
#endif

    #region Min
    /// <inheritdoc cref="Enumerable.Min(IEnumerable{decimal})"/>
    public static decimal Min([NonDefaultableStruct] this ReadOnlyArray<decimal> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{decimal?})"/>
    public static decimal? Min([NonDefaultableStruct] this ReadOnlyArray<decimal?> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{double})"/>
    public static double Min([NonDefaultableStruct] this ReadOnlyArray<double> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{double?})"/>
    public static double? Min([NonDefaultableStruct] this ReadOnlyArray<double?> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{float})"/>
    public static float Min([NonDefaultableStruct] this ReadOnlyArray<float> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{float?})"/>
    public static float? Min([NonDefaultableStruct] this ReadOnlyArray<float?> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{int})"/>
    public static int Min([NonDefaultableStruct] this ReadOnlyArray<int> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{int?})"/>
    public static int? Min([NonDefaultableStruct] this ReadOnlyArray<int?> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{long})"/>
    public static long Min([NonDefaultableStruct] this ReadOnlyArray<long> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min(IEnumerable{long?})"/>
    public static long? Min([NonDefaultableStruct] this ReadOnlyArray<long?> source) => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource})"/>
    public static TSource? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Min();

    /// <inheritdoc cref="Enumerable.Min{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult}})"/>
    public static TResult? Min<TSource, TResult>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                 Func<TSource, TResult> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, decimal})"/>
    public static decimal Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, decimal?})"/>
    public static decimal? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal?> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, double})"/>
    public static double Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, double> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, double?})"/>
    public static double? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                       Func<TSource, double?> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, float})"/>
    public static float Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, float> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, float?})"/>
    public static float? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, float?> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, int})"/>
    public static int Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                   Func<TSource, int> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, int?})"/>
    public static int? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, int?> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, long})"/>
    public static long Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, long> selector)
        => source._array.Min(selector);

    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, Func{TSource, long?})"/>
    public static long? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, long?> selector)
        => source._array.Min(selector);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource}, IComparer{TSource}?)"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource? Min<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        IComparer<TSource>? comparer)
        => source._array.Min(comparer);
#endif
    #endregion

#if NET6_0_OR_GREATER
    #region MinBy
    /// <inheritdoc cref="Enumerable.MinBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource? MinBy<TSource, TKey>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                Func<TSource, TKey> keySelector)
        => source._array.MinBy(keySelector);

    /// <inheritdoc cref="Enumerable.MinBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>
    [return: MaybeNull, MaybeDefault]
    public static TSource? MinBy<TSource, TKey>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        => source._array.MinBy(keySelector, comparer);
    #endregion
#endif

#if NET7_0_OR_GREATER
    #region Order
    /// <inheritdoc cref="Enumerable.Order{T}(IEnumerable{T})"/>
    public static IOrderedEnumerable<TSource> Order<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Order();

    /// <inheritdoc cref="Enumerable.Order{T}(IEnumerable{T}, IComparer{T}?)"/>
    public static IOrderedEnumerable<TSource> Order<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                             IComparer<TSource>? comparer)
        => source._array.Order(comparer);
    #endregion
#endif

    #region OrderBy
    /// <inheritdoc cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, TKey> keySelector)
        => source._array.OrderBy(keySelector);

    /// <inheritdoc cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>
    public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, IComparer<TKey>? keyComparer)
        => source._array.OrderBy(keySelector, keyComparer);
    #endregion

    #region OrderByDescending
    /// <inheritdoc cref="Enumerable.OrderByDescending{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, TKey> keySelector)
        => source._array.OrderByDescending(keySelector);

    /// <inheritdoc cref="Enumerable.OrderByDescending{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>
    public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, IComparer<TKey>? keyComparer)
        => source._array.OrderByDescending(keySelector, keyComparer);
    #endregion

#if NET7_0_OR_GREATER
    #region OrderDescending
    /// <inheritdoc cref="Enumerable.OrderDescending{T}(IEnumerable{T})"/>
    public static IOrderedEnumerable<TSource> OrderDescending<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.OrderDescending();

    /// <inheritdoc cref="Enumerable.OrderDescending{T}(IEnumerable{T}, IComparer{T}?)"/>
    public static IOrderedEnumerable<TSource> OrderDescending<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, IComparer<TSource>? comparer)
        => source._array.OrderDescending(comparer);
    #endregion
#endif

#if NET471_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP
    #region Prepend
    /// <inheritdoc cref="Enumerable.Prepend{TSource}(IEnumerable{TSource}, TSource)"/>
    public static IEnumerable<TSource> Prepend<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, TSource element)
        => source._array.Prepend(element);
    #endregion
#endif

    #region Reverse
    /// <inheritdoc cref="Enumerable.Reverse{TSource}(IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Reverse<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Reverse();
    #endregion

    #region Select
    /// <summary>
    /// Maps a selector over the current <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> was <see langword="null"/>.</exception>
    public static IEnumerable<U> Select<T, U>(
            [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, U> selector)
        => array.ThrowIfArgDefault(nameof(array))._array.Select(selector);

    /// <summary>
    /// Maps a selector over the values and corresponding indices of the current <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> was <see langword="null"/>.</exception>
    public static IEnumerable<U> Select<T, U>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, long, U> selector)
    {
        array.ThrowIfArgDefault(nameof(array));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        for (long i = 0; i < array.LongLength; i++) yield return selector(array[i], i);
    }
    #endregion

    #region SelectMany
    /// <summary>
    /// Maps a selector over the current <see cref="ReadOnlyArray{T}"/> and flattens the resulting collection by
    /// one level.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> was <see langword="null"/>.</exception>
    public static IEnumerable<U> SelectMany<T, U>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, IEnumerable<U>> selector)
        => array.ThrowIfArgDefault(nameof(array))._array.SelectMany(selector);

    /// <inheritdoc cref="SelectMany{T, U}(ReadOnlyArray{T}, Func{T, long, IEnumerable{U}})"/>
    public static IEnumerable<U> SelectMany<T, U>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, int, IEnumerable<U>> selector)
        => array.ThrowIfArgDefault(nameof(array))._array.SelectMany(selector);

    /// <summary>
    /// Maps a selector over this instance's values and corresponding indices and flattens the resulting collection
    /// by one level.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="selector"/> was <see langword="null"/>.</exception>
    public static IEnumerable<U> SelectMany<T, U>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, long, IEnumerable<U>> selector)
    {
        array.ThrowIfArgDefault(nameof(array));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        for (long l = 0; l < array.LongLength; l++)
        {
            var coll = selector(array[l], l);
            foreach (var u in coll) yield return u;
        }
    }
    #endregion

    #region SequenceEqual
    /// <inheritdoc cref="SequenceEqual{TParent, TChild}(IEnumerable{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent, TChild>(
            this IEnumerable<TParent> source,
            [NonDefaultableStruct] ReadOnlyArray<TChild> other)
    where TChild : TParent
        => source.SequenceEqual(other.ToParent<TChild, TParent>());


    /// <summary>
    /// Determines if the current <see cref="IEnumerable{T}"/> is equal to the specified <see cref="ReadOnlyArray{T}"/>
    /// by comparing their elements.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="source"></param>
    /// <param name="other"></param>
    /// <param name="elementComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare the elements, or <see langword="null"/> to use the
    /// default equality comparer for type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> was <see langword="null"/>.</exception>
    public static bool SequenceEqual<TParent, TChild>(
            this IEnumerable<TParent> source,
            [NonDefaultableStruct] ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? elementComparer)
    where TChild : TParent
        => source.SequenceEqual(other.ToParent<TChild, TParent>(), elementComparer);

    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> is equal to the specified
    /// <see cref="IEnumerable{T}"/> by comparing their elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="other"></param>
    /// <param name="elementComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare the elements, or <see langword="null"/> to use the
    /// default equality comparer for type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> was <see langword="null"/>.</exception>
    public static bool SequenceEqual<T>(
            [NonDefaultableStruct] this ReadOnlyArray<T> array,
            IEnumerable<T> other, IEqualityComparer<T>? elementComparer = null)
        => array.ThrowIfArgDefault(nameof(array))._array
                .SequenceEqual(
                    other is null ? throw new ArgumentNullException(nameof(other)) : other, elementComparer);

    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> is equal to the other specified
    /// <see cref="ReadOnlyArray{T}"/> by comparing their elements.
    /// </summary>
    /// <param name="elementComparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare the elements, or <see langword="null"/> to use the
    /// default equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <inheritdoc cref="SequenceEqual{TParent, TChild}(ReadOnlyArray{TParent}, ImmutableArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> array,
            [NonDefaultableStruct] ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? elementComparer = null)
    where TChild : TParent
        => array.ThrowIfArgDefault(nameof(array))._array
                .SequenceEqual(
                    (other.ThrowIfArgDefault(nameof(other))._array as IEnumerable<TParent>)!,
                    elementComparer);

    /// <summary>
    /// Determines if the current <see cref="ReadOnlyArray{T}"/> is equal to the specified
    /// <see cref="ImmutableArray{T}"/> by comparing their elements.
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
            [NonDefaultableStruct] this ReadOnlyArray<TParent> array,
            [NonDefaultableStruct] ImmutableArray<TChild> other, IEqualityComparer<TParent>? elementComparer = null)
    where TChild : TParent
    {
        array.ThrowIfArgDefault(nameof(array));
        other.ThrowIfArgDefault(nameof(other));

        if (array.Length != other.Length) return false;

        elementComparer ??= EqualityComparer<TParent>.Default;

        for (int i = 0; i < array.Length; i++) if (!elementComparer.Equals(array[i], other[i])) return false;
        return true;
    }
    #endregion

    #region Single
    /// <inheritdoc cref="Enumerable.Single{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    public static TSource Single<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, bool> predicate)
        => source._array.Single(predicate);

    /// <inheritdoc cref="Enumerable.Single{TSource}(IEnumerable{TSource})"/>
    public static TSource Single<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.Single();
    #endregion

    #region SingleOrDefault
    /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource})"/>
    public static TSource? SingleOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.SingleOrDefault();

    /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
    public static TSource? SingleOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                    Func<TSource, bool> predicate)
        => source._array.SingleOrDefault(predicate);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, TSource)"/>
    public static TSource SingleOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                   TSource defaultValue)
        => source._array.SingleOrDefault(defaultValue);

    /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool}, TSource)"/>
    public static TSource SingleOrDefault<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                   Func<TSource, bool> predicate, TSource defaultValue)
        => source._array.SingleOrDefault(predicate, defaultValue);
#endif
    #endregion

    #region Skip
    // Included as instance methods
    #endregion

    #region SkipLast
    // Included as instance methods
    #endregion

    #region SkipWhile
    // Included as instance methods
    #endregion

    #region Sum
    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{decimal})"/>
    public static decimal Sum([NonDefaultableStruct] this ReadOnlyArray<decimal> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{decimal?})"/>
    public static decimal? Sum([NonDefaultableStruct] this ReadOnlyArray<decimal?> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{double})"/>
    public static double Sum([NonDefaultableStruct] this ReadOnlyArray<double> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{double?})"/>
    public static double? Sum([NonDefaultableStruct] this ReadOnlyArray<double?> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{float})"/>
    public static float Sum([NonDefaultableStruct] this ReadOnlyArray<float> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{float?})"/>
    public static float? Sum([NonDefaultableStruct] this ReadOnlyArray<float?> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{int})"/>
    public static int Sum([NonDefaultableStruct] this ReadOnlyArray<int> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{int?})"/>
    public static int? Sum([NonDefaultableStruct] this ReadOnlyArray<int?> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{long})"/>
    public static long Sum([NonDefaultableStruct] this ReadOnlyArray<long> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum(IEnumerable{long?})"/>
    public static long? Sum([NonDefaultableStruct] this ReadOnlyArray<long?> source) => source._array.Sum();

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, decimal})"/>
    public static decimal Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, decimal?})"/>
    public static decimal? Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                        Func<TSource, decimal?> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, double})"/>
    public static double Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, double> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, double?})"/>
    public static double? Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                       Func<TSource, double?> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, float})"/>
    public static float Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, float> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, float?})"/>
    public static float? Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                      Func<TSource, float?> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, int})"/>
    public static int Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                   Func<TSource, int> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, int?})"/>
    public static int? Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, int?> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, long})"/>
    public static long Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                    Func<TSource, long> selector)
        => source._array.Sum(selector);

    /// <inheritdoc cref="Enumerable.Sum{TSource}(IEnumerable{TSource}, Func{TSource, long?})"/>
    public static long? Sum<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                     Func<TSource, long?> selector)
        => source._array.Sum(selector);
    #endregion

    #region Take
    // Included as instance methods
    #endregion

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    #region TakeLast
    // Included as instance methods
    #endregion
#endif

    #region TakeWhile
    // Included as instance methods
    #endregion

    #region ToArray
    /// <summary>
    /// Creates an array from the current <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TSource[] ToArray<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source.ThrowIfArgDefault(nameof(source)).GetClone();
    #endregion

    #region ToDictionary
    /// <inheritdoc cref="Enumerable.ToDictionary{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> source, Func<TSource, TKey> keySelector)
    where TKey : notnull
        => source._array.ToDictionary(keySelector);

    /// <inheritdoc cref="Enumerable.ToDictionary{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
        Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    where TKey : notnull
        => source._array.ToDictionary(keySelector, comparer);

    /// <inheritdoc cref="Enumerable.ToDictionary{TSource, TKey, TValue}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TValue})"/>
    public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
        Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
    where TKey : notnull
        => source._array.ToDictionary(keySelector, elementSelector);

    /// <inheritdoc cref="Enumerable.ToDictionary{TSource, TKey, TValue}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TValue}, IEqualityComparer{TKey}?)"/>
    public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(
        [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
        Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey>? comparer)
    where TKey : notnull
        => source._array.ToDictionary(keySelector, elementSelector, comparer);
    #endregion

#if NET472_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
    #region ToHashSet
    /// <inheritdoc cref="Enumerable.ToHashSet{TSource}(IEnumerable{TSource})"/>
    public static HashSet<TSource> ToHashSet<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.ToHashSet();

    /// <inheritdoc cref="Enumerable.ToHashSet{TSource}(IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static HashSet<TSource> ToHashSet<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                      IEqualityComparer<TSource>? comparer)
        => source._array.ToHashSet(comparer);
    #endregion
#endif

    #region ToImmutable
    /// <inheritdoc cref="ImmutableArray.ToImmutableArray{TSource}(IEnumerable{TSource})"/>
    public static ImmutableArray<TSource> ToImmutableArray<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.ToImmutableArray();

    /// <inheritdoc cref="ImmutableHashSet.ToImmutableHashSet{TSource}(IEnumerable{TSource})"/>
    public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.ToImmutableHashSet();

    /// <inheritdoc cref="ImmutableHashSet.ToImmutableHashSet{TSource}(IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source, IEqualityComparer<TSource>? comparer)
        => source._array.ToImmutableHashSet(comparer);

    /// <inheritdoc cref="ImmutableList.ToImmutableList{TSource}(IEnumerable{TSource})"/>
    public static ImmutableList<TSource> ToImmutableList<TSource>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source)
        => source._array.ToImmutableList();
    #endregion

    #region ToList
    /// <summary>
    /// Creates a <see cref="List{T}"/> from the current <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<TSource> ToList<TSource>(this ReadOnlyArray<TSource> source)
        => source.ThrowIfArgDefault(nameof(source))._array.ToList();
    #endregion

    #region ToLookup
    /// <inheritdoc cref="Enumerable.ToLookup{TSource, TKey, TElement}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}?)"/>
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer)
        => source._array.ToLookup(keySelector, elementSelector, comparer);

    /// <inheritdoc cref="Enumerable.ToLookup{TSource, TKey, TElement}(IEnumerable{TSource}, Func{TSource, TKey}, Func{TSource, TElement})"/>
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        => source._array.ToLookup(keySelector, elementSelector);

    /// <inheritdoc cref="Enumerable.ToLookup{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"/>
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        => source._array.ToLookup(keySelector, comparer);

    /// <inheritdoc cref="Enumerable.ToLookup{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> source,
            Func<TSource, TKey> keySelector)
        => source._array.ToLookup(keySelector);
    #endregion

#if NET6_0_OR_GREATER
    #region TryGetNonEnumeratedCount
    /// <inheritdoc cref="Enumerable.TryGetNonEnumeratedCount{TSource}(IEnumerable{TSource}, out int)"/>
    public static bool TryGetNonEnumeratedCount<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                         out int count)
        => Try.Success(out count, source._array.Length);
    #endregion
#endif

    #region Union
    /// <inheritdoc cref="Union{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Union<TParent, TChild>(this IEnumerable<TParent> source,
                                                              [NonDefaultableStruct] ReadOnlyArray<TChild> other)
    where TChild : TParent
        => source.Union(other.ToParent<TChild, TParent>());

    /// <inheritdoc cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
    public static IEnumerable<TSource> Union<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                      IEnumerable<TSource> other)
        => source._array.Union(other);

    /// <inheritdoc cref="Union{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Union<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> source,
            [NonDefaultableStruct] ReadOnlyArray<TChild> other)
    where TChild : TParent
        => source._array.Union(other.ToParent<TChild, TParent>());

    /// <inheritdoc cref="Union{TParent, TChild}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    public static IEnumerable<TParent> Union<TParent, TChild>(this IEnumerable<TParent> source,
                                                              [NonDefaultableStruct] ReadOnlyArray<TChild> other,
                                                              IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => source.Union(other.ToParent<TChild, TParent>(), comparer);

    /// <inheritdoc cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)"/>
    public static IEnumerable<TSource> Union<TSource>([NonDefaultableStruct] this ReadOnlyArray<TSource> source,
                                                      IEnumerable<TSource> other,
                                                      IEqualityComparer<TSource>? comparer)
        => source._array.Union(other, comparer);

    /// <summary>
    /// Computes the set union of the current sequence and another sequence of a child type.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current sequence.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <param name="source">The current sequence.</param>
    /// <param name="other">Another sequence to compute the set union with.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare elements, or <see langword="null"/> to use the default
    /// equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns>The set union of <paramref name="source"/> and <paramref name="other"/>.</returns>
    public static IEnumerable<TParent> Union<TParent, TChild>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> source,
            [NonDefaultableStruct] ReadOnlyArray<TChild> other,
            IEqualityComparer<TParent>? comparer)
    where TChild : TParent
        => source._array.Union(other.ToParent<TChild, TParent>(), comparer);
    #endregion

#if NET6_0_OR_GREATER
    #region UnionBy
    /// <inheritdoc cref="UnionBy{TParent, TChild, TKey}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, Func{TParent, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TParent> UnionBy<TParent, TChild, TKey>(
            this IEnumerable<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second, Func<TParent, TKey> keySelector)
    where TChild : TParent
        => first.UnionBy(second.ToParent<TChild, TParent>(), keySelector);

    /// <inheritdoc cref="Enumerable.UnionBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TSource}, Func{TSource, TKey})"/>
    public static IEnumerable<TSource> UnionBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
        => first._array.UnionBy(second, keySelector);

    /// <inheritdoc cref="UnionBy{TParent, TChild, TKey}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, Func{TParent, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TParent> UnionBy<TParent, TChild, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second, Func<TParent, TKey> keySelector)
    where TChild : TParent
        => first._array.UnionBy(second.ToParent<TChild, TParent>(), keySelector);

    /// <inheritdoc cref="UnionBy{TParent, TChild, TKey}(ReadOnlyArray{TParent}, ReadOnlyArray{TChild}, Func{TParent, TKey}, IEqualityComparer{TKey}?)"/>
    public static IEnumerable<TParent> UnionBy<TParent, TChild, TKey>(
            this IEnumerable<TParent> first,
            ReadOnlyArray<TChild> second, Func<TParent, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    where TChild : TParent
        => first.UnionBy(second.ToParent<TChild, TParent>(), keySelector, comparer);

    /// <inheritdoc cref="Enumerable.UnionBy{TSource, TKey}(IEnumerable{TSource}, IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)"
    public static IEnumerable<TSource> UnionBy<TSource, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TSource> first,
            IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        => first._array.UnionBy(second, keySelector, comparer);

    /// <summary>
    /// Computes the set union of the current sequence and another sequence of a child type by mapping the elements
    /// of the sequences to a third type for comparison.
    /// </summary>
    /// <typeparam name="TParent">The element type of the current sequence.</typeparam>
    /// <typeparam name="TChild">
    /// A subtype of <typeparamref name="TParent"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <typeparam name="TKey">The type elements will be mapped to for comparison.</typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">Another sequence to compute the set union with.</param>
    /// <param name="keySelector">The selector to use to map sequence elements to keys.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare keys, or <see langword="null"/> to use the default
    /// equality comparer for type <typeparamref name="TKey"/>.
    /// </param>
    /// <returns>The set union of <paramref name="first"/> and <paramref name="second"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> was <see langword="null"/>.</exception>
    public static IEnumerable<TParent> UnionBy<TParent, TChild, TKey>(
            [NonDefaultableStruct] this ReadOnlyArray<TParent> first,
            [NonDefaultableStruct] ReadOnlyArray<TChild> second,
            Func<TParent, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    where TChild : TParent
        => first._array.UnionBy(second.ToParent<TChild, TParent>(), keySelector, comparer);
    #endregion
#endif

    #region Where
    /// <summary>
    /// Filters the current <see cref="ReadOnlyArray{T}"/> based on a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    public static IEnumerable<T> Where<T>([NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, bool> predicate)
        => array.ThrowIfArgDefault(nameof(array))._array.Where(predicate);

    /// <summary>
    /// Filters the current <see cref="ReadOnlyArray{T}"/> based on a predicate. Each element's index is used in the
    /// logic of the predicate function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">The current instance was the default.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    public static IEnumerable<T> Where<T>(
        [NonDefaultableStruct] this ReadOnlyArray<T> array, Func<T, long, bool> predicate)
    {
        array.ThrowIfArgDefault(nameof(array));
        for (long l = 0; l < array.LongLength; l++) if (predicate(array[l], l)) yield return array[l];
    }
    #endregion

    #region Zip
    /// <inheritdoc cref="Zip{TFirst, TSecondParent, TSecondChild, TResult}(ReadOnlyArray{TFirst}, ReadOnlyArray{TSecondChild}, Func{TFirst, TSecondParent, TResult})"/>
    public static IEnumerable<TResult> Zip<TFirst, TSecondParent, TSecondChild, TResult>(
            [NonDefaultableStruct] this IEnumerable<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecondChild> second,
            Func<TFirst, TSecondParent, TResult> resultSelector)
    where TSecondChild : TSecondParent
        => first.Zip(second.ToParent<TSecondChild, TSecondParent>(), resultSelector);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>
    public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        => first._array.Zip(second, resultSelector);

    /// <summary>
    /// Zips the current sequence together with another using the specifed result selector function.
    /// </summary>
    /// <typeparam name="TFirst">The element type of the current sequence.</typeparam>
    /// <typeparam name="TSecondParent">The second parameter type of the result selector function.</typeparam>
    /// <typeparam name="TSecondChild">
    /// A subtype of <typeparamref name="TSecondParent"/> that is the element type of the other sequence.
    /// </typeparam>
    /// <typeparam name="TResult">The element type of the result sequence.</typeparam>
    /// <param name="first">The current sequence.</param>
    /// <param name="second">The other sequence.</param>
    /// <param name="resultSelector">The result selector to use to zip elements of the sequences into results.</param>
    /// <returns>
    /// The current sequence zipped with <paramref name="second"/> using <paramref name="resultSelector"/>.
    /// </returns>
    public static IEnumerable<TResult> Zip<TFirst, TSecondParent, TSecondChild, TResult>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecondChild> second,
            Func<TFirst, TSecondParent, TResult> resultSelector)
    where TSecondChild : TSecondParent
        => first._array.Zip(second.ToParent<TSecondChild, TSecondParent>(), resultSelector);

#if NETCOREAPP3_1_OR_GREATER || NET5_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond}(IEnumerable{TFirst}, IEnumerable{TSecond})"/>
    public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(
            this IEnumerable<TFirst> first, [NonDefaultableStruct] ReadOnlyArray<TSecond> second)
        => first.Zip(second._array);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond}(IEnumerable{TFirst}, IEnumerable{TSecond})"/>
    public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first, IEnumerable<TSecond> second)
        => first._array.Zip(second);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond}(IEnumerable{TFirst}, IEnumerable{TSecond})"/>
    public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second)
        => first._array.Zip(second._array);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>
    public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        => first.Zip(second._array, resultSelector);
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            this IEnumerable<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second, IEnumerable<TThird> third)
        => first.Zip(second._array, third);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, [NonDefaultableStruct] ReadOnlyArray<TThird> third)
        => first.Zip(second, third._array);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            this IEnumerable<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second, [NonDefaultableStruct] ReadOnlyArray<TThird> third)
        => first.Zip(second._array, third._array);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            IEnumerable<TSecond> second, IEnumerable<TThird> third)
        => first._array.Zip(second, third);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second, IEnumerable<TThird> third)
        => first._array.Zip(second._array, third);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            IEnumerable<TSecond> second, [NonDefaultableStruct] ReadOnlyArray<TThird> third)
        => first._array.Zip(second, third._array);

    /// <inheritdoc cref="Enumerable.Zip{TFirst, TSecond, TThird}(IEnumerable{TFirst}, IEnumerable{TSecond}, IEnumerable{TThird})"/>
    public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
            [NonDefaultableStruct] this ReadOnlyArray<TFirst> first,
            [NonDefaultableStruct] ReadOnlyArray<TSecond> second, [NonDefaultableStruct] ReadOnlyArray<TThird> third)
        => first._array.Zip(second._array, third._array);
#endif
    #endregion
    #endregion

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

/// <summary>
/// Represents a read-only array.
/// </summary>
/// <remarks>
/// This struct creates a thin, read-only wrapper around an array of type <typeparamref name="T"/>.
/// The array <i>can still be modified</i>, just not through the wrapper.
/// <para/>
/// This type returns readonly references to its elements from its indexers, so they are valid return values in
/// <see langword="ref"/> <see langword="readonly"/> contexts.
/// </remarks>
/// <typeparam name="T">The element type of the array.</typeparam>
public readonly struct ReadOnlyArray<T>
    : IDefaultableStruct,
      IList<T>, IReadOnlyContainer<T>, IReadOnlyList<T>, ISpecifiedEnumerable<ArrayEnumerator<T>, T>
{
    #region Constants
    /// <summary>
    /// Gets an empty <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <remarks>
    /// This array wraps the result of a call to <see cref="Array.Empty{T}()"/> to avoid zero-length allocations.
    /// </remarks>
    public static ReadOnlyArray<T> Empty => new(Array.Empty<T>());
    #endregion

    #region Properties And Fields
    /// <inheritdoc/>
    public bool IsDefault => _array is null;

    /// <summary>
    /// Gets the length of the wrapped array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public int Length => GetCount();

    /// <summary>
    /// Gets the length of the wrapped array as a 64-bit integer.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public long LongLength => ThrowIfDefault()._array.LongLength;

    [DoesNotReturnIfInstanceDefault]
    int ICollection<T>.Count => GetCount();

    [DoesNotReturnIfInstanceDefault]
    int IReadOnlyCollection<T>.Count => GetCount();

    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[int index] => ref ThrowIfDefault()._array[index];

    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[long index] => ref ThrowIfDefault()._array[index];

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[Index index] => ref this[(LongIndex)index];
#endif

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[LongIndex index]
    {
        get
        {
            ThrowIfDefault();
            var offset = index.GetOffset(LongLength);
            return ref _array[offset];
        }
    }

    [DoesNotReturnIfInstanceDefault]
    T IReadOnlyList<T>.this[int index] => GetAtIndex(index);

    [DoesNotReturnIfInstanceDefault]
    T IList<T>.this[int index]
    {
        get => GetAtIndex(index);
        set => throw Enumerables.MutationNotSupported;
    }

    internal readonly T[] _array;

    bool ICollection<T>.IsReadOnly => true;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="ReadOnlyArray{T}"/> struct wrapping the specified array.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// <para/>
    /// Passing <see langword="null"/> as the <paramref name="array"/> parameter will result in the default instance
    /// of this type.
    /// </remarks>
    /// <param name="array">The array to construct a readonly wrapper for.</param>
    [NotDefaultIfNotDefault(nameof(array))]
    public ReadOnlyArray(T[]? array) { _array = array!; }
    #endregion

    #region Methods
    #region IEnumerable
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    [DoesNotReturnIfInstanceDefault]
    public ArrayEnumerator<T> GetEnumerator() => new(ThrowIfDefault()._array);

    [DoesNotReturnIfInstanceDefault]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [DoesNotReturnIfInstanceDefault]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region IList
    #region Public
    /// <summary>
    /// Determines the index of a specified element in the <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <param name="item">The object to locate in the array.</param>
    /// <returns>
    /// The index of <paramref name="item"/> if found in the array; otherwise -1;
    /// </returns>
    [DoesNotReturnIfInstanceDefault]
    public int IndexOf(T item) => (ThrowIfDefault()._array as IList<T>).IndexOf(item);

    /// <summary>
    /// Copies the elements of the current <see cref="ReadOnlyArray{T}"/> to the specified array starting at the
    /// specified index.
    /// </summary>
    /// <param name="array">
    /// The array that is the destination of elements copied from this <see cref="ReadOnlyArray{T}"/>.
    /// </param>
    /// <inheritdoc/>
    [DoesNotReturnIfInstanceDefault]
    public void CopyTo(T[] array, int arrayIndex) => ThrowIfDefault()._array.CopyTo(array, arrayIndex);
    #endregion

    #region Not Supported
    [DoesNotReturn]
    void IList<T>.Insert(int index, T item)
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    void IList<T>.RemoveAt(int index)
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    void ICollection<T>.Add(T item)
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    void ICollection<T>.Clear()
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    bool ICollection<T>.Remove(T item)
    {
        throw Enumerables.MutationNotSupported;
    }
    #endregion
    #endregion

    #region LINQ
    #region Chunk
    /// <inheritdoc cref="Chunk(long)"/>
    public IEnumerable<ReadOnlyArraySlice<T>> Chunk(int size) => Chunk((long)size);

    /// <summary>
    /// Separates the current sequence into a series of chunks of maximum size <paramref name="size"/>.
    /// </summary>
    /// <param name="size">The maximum size of the chunks to separate the current sequence into.</param>
    /// <returns>
    /// A sequence of chunks of maximum size <paramref name="size"/>.
    /// All chunks returned except the last will have size exactly <paramref name="size"/>, where the last chunk
    /// may be shorter.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
    public IEnumerable<ReadOnlyArraySlice<T>> Chunk(long size)
        => size < 1
            ? throw new ArgumentOutOfRangeException(nameof(size), size, $"Size must be at least 1.")
            : ChunkUnchecked(size);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<ReadOnlyArraySlice<T>> ChunkUnchecked(long size)
    {
        if (size >= LongLength) yield return (ReadOnlyArraySlice<T>)this;
        else
        {
            long offset = 0;
            while (offset < LongLength)
            {
                var count = Math.Min(LongLength - offset, size);
                yield return new(_array, offset, count);
                offset += count;
            }
        }
    }
    #endregion

    #region Slicing
    /// <inheritdoc cref="Skip(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Skip(int count) => Skip((long)count);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array offset by the given number of elements.
    /// </summary>
    /// <param name="count"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The count was negative or out of range for the array.
    /// </exception>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Skip(long count) => ((ReadOnlyArraySlice<T>)this).Skip(count);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array with the given number of elements removed
    /// from the end.
    /// </summary>
    /// <param name="count"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The offset was negative or out of range for the array.
    /// </exception>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipLast(long count) => ((ReadOnlyArraySlice<T>)this).SkipLast(count);

    /// <inheritdoc cref="SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

    /// <inheritdoc cref="SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, int, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array with elements skipped until the specified
    /// predicate fails.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, long, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="Take(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(Range range) => ((ReadOnlyArraySlice<T>)this).Take(range);
#endif

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> representing the given range of elements from the array.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(LongRange range) => ((ReadOnlyArraySlice<T>)this).Take(range);

    /// <inheritdoc cref="Take(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(int count) => Take((long)count);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> representing the given number of elements from the beginning
    /// of the array.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(long count) => ((ReadOnlyArraySlice<T>)this).Take(count);

    /// <inheritdoc cref="TakeWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).TakeWhile(predicate);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing elements taken from this array starting at the
    /// beginning and ending when the specified predicate fails.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, long, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).TakeWhile(predicate);

    /// <inheritdoc cref="TakeLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(int count) => ((ReadOnlyArraySlice<T>)this).TakeLast(count);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing the specified number of elements taken from the
    /// end of the array.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(long count) => ((ReadOnlyArraySlice<T>)this).TakeLast(count);
    #endregion
    #endregion

    #region Equality
    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(ReadOnlyArray<T> other) => _array == other._array;

    /// <summary>
    /// Gets a hash code representing the current instance.
    /// </summary>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public override int GetHashCode() => _array.GetHashCode();
    #endregion

    #region Containment
    /// <summary>
    /// Determines whether the <see cref="ReadOnlyArray{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public bool Contains(T item) => (ThrowIfDefault()._array as IList<T>).Contains(item);
    #endregion

    #region Conversion
    /// <summary>
    /// Converts a readonly array of type <typeparamref name="TChild"/> to a readonly array of this type,
    /// using array covariance to avoid allocating another array.
    /// </summary>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    [return: NotDefaultIfNotDefault(nameof(array))]
    public static ReadOnlyArray<T> FromChild<TChild>(ReadOnlyArray<TChild> array) where TChild : class, T
        => new(array._array);

    /// <summary>
    /// Implicitly converts an array to a readonly wrapper for it.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// </remarks>
    /// <param name="array">The array to convert.</param>
    [return: NotDefaultIfNotDefault(nameof(array))]
    public static implicit operator ReadOnlyArray<T>(T[]? array) => new(array);

    /// <summary>
    /// Wraps the array in a <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// </remarks>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public ObjectModel.ReadOnlyCollection<T> ToReadOnlyCollection() => new(ThrowIfDefault()._array);
    #endregion

    #region Clone
    /// <summary>
    /// Gets a shallow copy of the array wrapped in this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public T[] GetClone() => Unsafe.As<T[]>(ThrowIfDefault()._array.Clone());
    #endregion

    #region Helpers
    /// <summary>
    /// Gets the item at the given index.
    /// </summary>
    /// <remarks>
    /// This method is merely for code reuse purposes.
    /// </remarks>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    private T GetAtIndex(int index) => ThrowIfDefault()._array[index];

    /// <summary>
    /// Gets the number of elements in this instance.
    /// </summary>
    /// <remarks>
    /// This method is merely for code reuse purposes.
    /// </remarks>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    private int GetCount() => ThrowIfDefault()._array.Length;

    /// <summary>
    /// Throws a <see cref="DefaultInstanceException"/> if this instance is the default.
    /// </summary>
    /// <returns>This instance.</returns>
    /// <exception cref="DefaultInstanceException">Thrown if this instance is the default.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyArray<T> ThrowIfDefault() => IsDefault ? throw new DefaultInstanceException() : this;
    #endregion
    #endregion
}
