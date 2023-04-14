using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

public interface ILinqImplementation<TParentCollection, TParent, TParentChunk,
                                     TChildCollection, TChild,
                                     TParent2Collection, TParent2,
                                     TChild2Collection, TChild2,
                                     TOtherElementCollection, TOtherElement,
                                     TResult,
                                     TDecimalCollection, TNullableDecimalCollection,
                                     TDoubleCollection, TNullableDoubleCollection,
                                     TFloatCollection, TNullableFloatCollection,
                                     TIntCollection, TNullableIntCollection,
                                     TLongCollection, TNullableLongCollection>
where TParentCollection : IEnumerable<TParent> where TParentChunk : IEnumerable<TParent>
where TChildCollection : IEnumerable<TChild>
where TChild : TParent
where TParent2Collection : IEnumerable<TParent2>
where TChild2Collection : IEnumerable<TChild2>
where TChild2 : TParent2
where TOtherElementCollection : IEnumerable<TOtherElement>
where TDecimalCollection : IEnumerable<decimal> where TNullableDecimalCollection : IEnumerable<decimal?>
where TDoubleCollection : IEnumerable<double> where TNullableDoubleCollection : IEnumerable<double?>
where TFloatCollection : IEnumerable<float> where TNullableFloatCollection : IEnumerable<float?>
where TIntCollection : IEnumerable<int> where TNullableIntCollection : IEnumerable<int?>
where TLongCollection : IEnumerable<long> where TNullableLongCollection : IEnumerable<long?>
{
    #region Aggregate
    public TParent Aggregate(TParentCollection current, Func<TParent, TParent, TParent> func);
    public TParent2 Aggregate(TParentCollection current, TParent2 seed, Func<TParent2, TParent, TParent2> func);
    public TResult Aggregate(TParentCollection current,
                             TParent2 seed,
                             Func<TParent2, TParent, TParent2> func, Func<TParent2, TResult> resultSelector);
    #endregion

    #region All
    public bool All(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region Any
    public bool Any(TParentCollection current);
    public bool Any(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region Append
    public IEnumerable<TParent> Append(TParentCollection current, TParent value);
    #endregion

    #region Average
    public decimal Average(TDecimalCollection current);
    public decimal? Average(TNullableDecimalCollection current);
    public double Average(TDoubleCollection current);
    public double? Average(TNullableDoubleCollection current);
    public float Average(TFloatCollection current);
    public float? Average(TNullableFloatCollection current);
    public double Average(TIntCollection current);
    public double? Average(TNullableIntCollection current);
    public double Average(TLongCollection current);
    public double? Average(TNullableLongCollection current);
    public decimal Average(TParentCollection current, Func<TParent, decimal> selector);
    public decimal? Average(TParentCollection current, Func<TParent, decimal?> selector);
    public double Average(TParentCollection current, Func<TParent, double> selector);
    public double? Average(TParentCollection current, Func<TParent, double?> selector);
    public float Average(TParentCollection current, Func<TParent, float> selector);
    public float? Average(TParentCollection current, Func<TParent, float?> selector);
    public double Average(TParentCollection current, Func<TParent, int> selector);
    public double? Average(TParentCollection current, Func<TParent, int?> selector);
    public double Average(TParentCollection current, Func<TParent, long> selector);
    public double? Average(TParentCollection current, Func<TParent, long?> selector);
    #endregion

    #region Chunk
    public IEnumerable<TParentChunk> Chunk(TParentCollection current, int size);
    #endregion

    #region Concat
    public IEnumerable<TParent> Concat(TParentCollection current, TChildCollection other);
    public IEnumerable<TParent> Concat(IEnumerable<TParent> current, TChildCollection other);
    public IEnumerable<TParent> Concat(TParentCollection first, IEnumerable<TParent> second);
    #endregion

    #region Contains
    public bool Contains(TParentCollection current, TParent value);
    public bool Contains(TParentCollection current, TParent value, IEqualityComparer<TParent>? comparer);
    #endregion

    #region Count
    public int Count(TParentCollection current);
    public int Count(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region DefaultIfEmpty
    public IEnumerable<TParent?> DefaultIfEmpty(TParentCollection current);
    public IEnumerable<TParent> DefaultIfEmpty(TParentCollection current, TParent defaultValue);
    #endregion

    #region Distinct
    public IEnumerable<TParent> Distinct(TParentCollection current);
    public IEnumerable<TParent> Distinct(TParentCollection current, IEqualityComparer<TParent>? comparer);
    #endregion

    #region DistinctBy
    public IEnumerable<TParent> DistinctBy(TParentCollection current, Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> DistinctBy(TParentCollection current, Func<TParent, TParent2> keySelector,
                                           IEqualityComparer<TParent2>? comparer);
    #endregion

    #region ElementAt
    public TParent ElementAt(TParentCollection current, int index);
    public TParent ElementAt(TParentCollection current, Index index);
    public TParent ElementAt(TParentCollection current, LongIndex index);
    #endregion

    #region ElementAtOrDefault
    public TParent? ElementAtOrDefault(TParentCollection current, int index);
    public TParent? ElementAtOrDefault(TParentCollection current, Index index);
    public TParent? ElementAtOrDefault(TParentCollection current, LongIndex index);
    #endregion

    #region Except
    public IEnumerable<TParent> Except(TParentCollection first, IEnumerable<TParent> second);
    public IEnumerable<TParent> Except(TParentCollection first, IEnumerable<TParent> second,
                                       IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Except(IEnumerable<TParent> first, TChildCollection second);
    public IEnumerable<TParent> Except(IEnumerable<TParent> first, TChildCollection second,
                                       IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Except(TParentCollection first, TChildCollection second);
    public IEnumerable<TParent> Except(TParentCollection first, TChildCollection second,
                                       IEqualityComparer<TParent>? comparer);
    #endregion

    #region ExceptBy
    public IEnumerable<TParent> ExceptBy(TParentCollection current, TChild2Collection keys,
                                          Func<TParent, TParent2> keySelector,
                                          IEqualityComparer<TParent2>? comparer);
    public IEnumerable<TParent> ExceptBy(TParentCollection current, TChild2Collection keys,
                                         Func<TParent, TParent2> keySelector);

    public IEnumerable<TParent> ExceptBy(IEnumerable<TParent> current, TChild2Collection keys,
                                         Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> ExceptBy(IEnumerable<TParent> current, TChild2Collection keys,
                                         Func<TParent, TParent2> keySelector,
                                         IEqualityComparer<TParent2>? comparer);

    public IEnumerable<TParent> ExceptBy(TParentCollection current, IEnumerable<TParent2> keys,
                                         Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> ExceptBy(TParentCollection current, IEnumerable<TParent2> keys,
                                         Func<TParent, TParent2> keySelector,
                                         IEqualityComparer<TParent2>? comparer);
    #endregion

    #region First
    public TParent First(TParentCollection current);
    public TParent First(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region FirstOrDefault
    public TParent? FirstOrDefault(TParentCollection current);
    public TParent? FirstOrDefault(TParentCollection current, Func<TParent, bool> predicate);
    public TParent FirstOrDefault(TParentCollection current, TParent defaultValue);
    public TParent FirstOrDefault(TParentCollection current, Func<TParent, bool> predicate, TParent defaultValue);
    #endregion

    #region GroupBy
    public IEnumerable<TResult> GroupBy(TParentCollection current,
                                        Func<TParent, TParent2> keySelector,
                                        Func<TParent, TOtherElement> elementSelector,
                                        Func<TParent2, IEnumerable<TOtherElement>, TResult> resultSelector);

    public IEnumerable<TResult> GroupBy(TParentCollection current,
                                        Func<TParent, TParent2> keySelector,
                                        Func<TParent, TOtherElement> elementSelector,
                                        Func<TParent2, IEnumerable<TOtherElement>, TResult> resultSelector,
                                        IEqualityComparer<TParent2>? comparer);

    public IEnumerable<IGrouping<TParent2, TParent>> GroupBy(TParentCollection current,
                                                             Func<TParent, TParent2> keySelector);

    public IEnumerable<IGrouping<TParent2, TParent>> GroupBy(TParentCollection current,
                                                             Func<TParent, TParent2> keySelector,
                                                             IEqualityComparer<TParent2>? comparer);

    public IEnumerable<IGrouping<TParent2, TOtherElement>> GroupBy(TParentCollection current,
                                                                   Func<TParent, TParent2> keySelector,
                                                                   Func<TParent, TOtherElement> elementSelector);

    public IEnumerable<IGrouping<TParent2, TOtherElement>> GroupBy(TParentCollection current,
                                                                   Func<TParent, TParent2> keySelector,
                                                                   Func<TParent, TOtherElement> elementSelector,
                                                                   IEqualityComparer<TParent2>? comparer);

    public IEnumerable<TResult> GroupBy(TParentCollection current,
                                        Func<TParent, TParent2> keySelector,
                                        Func<TParent2, IEnumerable<TParent>, TResult> resultSelector);

    public IEnumerable<TResult> GroupBy(TParentCollection current,
                                        Func<TParent, TParent2> keySelector,
                                        Func<TParent2, IEnumerable<TParent>, TResult> resultSelector,
                                        IEqualityComparer<TParent2>? comparer);
    #endregion

    #region GroupJoin
    public IEnumerable<TResult> GroupJoin(TParentCollection outer, IEnumerable<TParent2> inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector);

    public IEnumerable<TResult> GroupJoin(TParentCollection outer, IEnumerable<TParent2> inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector,
                                          IEqualityComparer<TOtherElement>? comparer);

    public IEnumerable<TResult> GroupJoin(IEnumerable<TParent> outer, TChild2Collection inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector);

    public IEnumerable<TResult> GroupJoin(IEnumerable<TParent> outer, TChild2Collection inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector,
                                          IEqualityComparer<TOtherElement>? comparer);

    public IEnumerable<TResult> GroupJoin(TParentCollection outer, TChild2Collection inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector);

    public IEnumerable<TResult> GroupJoin(TParentCollection outer, TChild2Collection inner,
                                          Func<TParent, TOtherElement> outerKeySelector,
                                          Func<TParent2, TOtherElement> innerKeySelector,
                                          Func<TParent, IEnumerable<TParent2>, TResult> resultSelector,
                                          IEqualityComparer<TOtherElement>? comparer);
    #endregion

    #region Intersect
    public IEnumerable<TParent> Intersect(IEnumerable<TParent> first, TChildCollection second);
    public IEnumerable<TParent> Intersect(IEnumerable<TParent> first, TChildCollection second,
                                          IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Intersect(TParentCollection first, IEnumerable<TParent> second);
    public IEnumerable<TParent> Intersect(TParentCollection first, IEnumerable<TParent> second,
                                          IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Intersect(TParentCollection first, TChildCollection second);
    public IEnumerable<TParent> Intersect(TParentCollection first, TChildCollection second,
                                          IEqualityComparer<TParent>? comparer);
    #endregion

    #region IntersectBy
    public IEnumerable<TParent> IntersectBy(TParentCollection first, IEnumerable<TParent2> second,
                                            Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> IntersectBy(TParentCollection first, IEnumerable<TParent2> second,
                                            Func<TParent, TParent2> keySelector,
                                            IEqualityComparer<TParent2>? comparer);

    public IEnumerable<TParent> IntersectBy(TParentCollection first, TChild2Collection second,
                                            Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> IntersectBy(TParentCollection first, TChild2Collection second,
                                            Func<TParent, TParent2> keySelector,
                                            IEqualityComparer<TParent2>? comparer);

    public IEnumerable<TParent> IntersectBy(IEnumerable<TParent> first, TChild2Collection second,
                                            Func<TParent, TParent2> keySelector);
    public IEnumerable<TParent> IntersectBy(IEnumerable<TParent> first, TChild2Collection second,
                                            Func<TParent, TParent2> keySelector,
                                            IEqualityComparer<TParent2>? comparer);
    #endregion

    #region Join
    public IEnumerable<TResult> Join(TParentCollection outer, IEnumerable<TParent2> inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector);

    public IEnumerable<TResult> Join(TParentCollection outer, IEnumerable<TParent2> inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector,
                                     IEqualityComparer<TOtherElement>? comparer);

    public IEnumerable<TResult> Join(IEnumerable<TParent> outer, TChild2Collection inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector);

    public IEnumerable<TResult> Join(IEnumerable<TParent> outer, TChild2Collection inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector,
                                     IEqualityComparer<TOtherElement>? comparer);

    public IEnumerable<TResult> Join(TParentCollection outer, TChild2Collection inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector);

    public IEnumerable<TResult> Join(TParentCollection outer, TChild2Collection inner,
                                     Func<TParent, TOtherElement> outerKeySelector,
                                     Func<TParent2, TOtherElement> innerKeySelector,
                                     Func<TParent, TParent2, TResult> resultSelector,
                                     IEqualityComparer<TOtherElement>? comparer);
    #endregion

    #region Last
    public TParent Last(TParentCollection current);
    public TParent Last(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region LastOrDefault
    public TParent? LastOrDefault(TParentCollection current);
    public TParent? LastOrDefault(TParentCollection current, Func<TParent, bool> predicate);
    public TParent LastOrDefault(TParentCollection current, TParent defaultValue);
    public TParent LastOrDefault(TParentCollection current, Func<TParent, bool> predicate, TParent defaultValue);
    #endregion

    #region LongCount
    public long LongCount(TParentCollection current);
    public long LongCount(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region Max
    public decimal Max(TDecimalCollection current);
    public decimal? Max(TNullableDecimalCollection current);
    public double Max(TDoubleCollection current);
    public double? Max(TNullableDoubleCollection current);
    public float Max(TFloatCollection current);
    public float? Max(TNullableFloatCollection current);
    public int Max(TIntCollection current);
    public int? Max(TNullableIntCollection current);
    public long Max(TLongCollection current);
    public long? Max(TNullableLongCollection current);
    public TParent? Max(TParentCollection current);
    public TParent2? Max(TParentCollection current, Func<TParent, TParent2> selector);
    public decimal Max(TParentCollection current, Func<TParent, decimal> selector);
    public decimal? Max(TParentCollection current, Func<TParent, decimal?> selector);
    public double Max(TParentCollection current, Func<TParent, double> selector);
    public double? Max(TParentCollection current, Func<TParent, double?> selector);
    public float Max(TParentCollection current, Func<TParent, float> selector);
    public float? Max(TParentCollection current, Func<TParent, float?> selector);
    public int Max(TParentCollection current, Func<TParent, int> selector);
    public int? Max(TParentCollection current, Func<TParent, int?> selector);
    public long Max(TParentCollection current, Func<TParent, long> selector);
    public long? Max(TParentCollection current, Func<TParent, long?> selector);
    public TParent? Max(TParentCollection current, IComparer<TParent>? comparer);
    #endregion

    #region MaxBy
    public TParent? MaxBy(TParentCollection current, Func<TParent, TParent2> selector);
    public TParent? MaxBy(TParentCollection current, Func<TParent, TParent2> selector, IComparer<TParent2> comparer);
    #endregion

    #region Min
    public decimal Min(TDecimalCollection current);
    public decimal? Min(TNullableDecimalCollection current);
    public double Min(TDoubleCollection current);
    public double? Min(TNullableDoubleCollection current);
    public float Min(TFloatCollection current);
    public float? Min(TNullableFloatCollection current);
    public int Min(TIntCollection current);
    public int? Min(TNullableIntCollection current);
    public long Min(TLongCollection current);
    public long? Min(TNullableLongCollection current);
    public TParent? Min(TParentCollection current);
    public TParent2? Min(TParentCollection current, Func<TParent, TParent2> selector);
    public decimal Min(TParentCollection current, Func<TParent, decimal> selector);
    public decimal? Min(TParentCollection current, Func<TParent, decimal?> selector);
    public double Min(TParentCollection current, Func<TParent, double> selector);
    public double? Min(TParentCollection current, Func<TParent, double?> selector);
    public float Min(TParentCollection current, Func<TParent, float> selector);
    public float? Min(TParentCollection current, Func<TParent, float?> selector);
    public int Min(TParentCollection current, Func<TParent, int> selector);
    public int? Min(TParentCollection current, Func<TParent, int?> selector);
    public long Min(TParentCollection current, Func<TParent, long> selector);
    public long? Min(TParentCollection current, Func<TParent, long?> selector);
    public TParent? Min(TParentCollection current, IComparer<TParent>? comparer);
    #endregion

    #region MinBy
    public TParent? MinBy(TParentCollection current, Func<TParent, TParent2> selector);
    public TParent? MinBy(TParentCollection current, Func<TParent, TParent2> selector, IComparer<TParent2> comparer);
    #endregion

    #region Order
    public IOrderedEnumerable<TParent> Order(TParentCollection current);
    public IOrderedEnumerable<TParent> Order(TParentCollection current, IComparer<TParent>? comparer);
    #endregion

    #region OrderBy
    public IOrderedEnumerable<TParent> OrderBy(TParentCollection current, Func<TParent, TParent2> selector);
    public IOrderedEnumerable<TParent> OrderBy(TParentCollection current, Func<TParent, TParent2> selector,
                                               IComparer<TParent2>? comparer);
    #endregion

    #region OrderByDescending
    public IOrderedEnumerable<TParent> OrderByDescending(TParentCollection current, Func<TParent, TParent2> selector);
    public IOrderedEnumerable<TParent> OrderByDescending(TParentCollection current, Func<TParent, TParent2> selector,
                                                         IComparer<TParent2>? comparer);
    #endregion

    #region OrderDescending
    public IOrderedEnumerable<TParent> OrderDescending(TParentCollection current);
    public IOrderedEnumerable<TParent> OrderDescending(TParentCollection current, IComparer<TParent>? comparer);
    #endregion

    #region Prepend
    public IEnumerable<TParent> Prepend(TParentCollection current, TParent value);
    #endregion

    #region Reverse
    public IEnumerable<TParent> Reverse(TParentCollection current);
    #endregion

    #region Select
    public IEnumerable<TParent2> Select(TParentCollection current, Func<TParent, TParent2> selector);
    public IEnumerable<TParent2> Select(TParentCollection current, Func<TParent, long, TParent2> selector);
    #endregion

    #region SelectMany
    public IEnumerable<TParent2> SelectMany(TParentCollection current, Func<TParent, IEnumerable<TParent2>> selector);
    public IEnumerable<TParent2> SelectMany(TParentCollection current, Func<TParent, long, IEnumerable<TParent2>> selector);
    #endregion

    #region SequenceEqual
    public bool SequenceEqual(IEnumerable<TParent> current, TChildCollection other);
    public bool SequenceEqual(IEnumerable<TParent> current, TChildCollection other,
                              IEqualityComparer<TParent>? comparer);
    public bool SequenceEqual(TParentCollection current, IEnumerable<TParent> other);
    public bool SequenceEqual(TParentCollection current, IEnumerable<TParent> other,
                              IEqualityComparer<TParent>? comparer);
    public bool SequenceEqual(TParentCollection current, TChildCollection other);
    public bool SequenceEqual(TParentCollection current, TChildCollection other, IEqualityComparer<TParent>? comparer);
    #endregion

    #region Single
    public TParent Single(TParentCollection current);
    public TParent Single(TParentCollection current, Func<TParent, bool> predicate);
    #endregion

    #region SingleOrDefault
    public TParent? SingleOrDefault(TParentCollection current);
    public TParent? SingleOrDefault(TParentCollection current, Func<TParent, bool> predicate);
    public TParent SingleOrDefault(TParentCollection current, TParent defaultValue);
    public TParent SingleOrDefault(TParentCollection current, Func<TParent, bool> predicate, TParent defaultValue);
    #endregion

    #region Skip
    public IEnumerable<TParent> Skip(TParentCollection current, int count);
    public IEnumerable<TParent> Skip(TParentCollection current, long count);
    #endregion

    #region SkipLast
    public IEnumerable<TParent> SkipLast(TParentCollection current, int count);
    public IEnumerable<TParent> SkipLast(TParentCollection current, long count);
    #endregion

    #region SkipWhile
    public IEnumerable<TParent> SkipWhile(TParentCollection current, Func<TParent, bool> predicate);
    public IEnumerable<TParent> SkipWhile(TParentCollection current, Func<TParent, long, bool> predicate);
    #endregion

    #region Sum
    public decimal Sum(TDecimalCollection current);
    public decimal? Sum(TNullableDecimalCollection current);
    public double Sum(TDoubleCollection current);
    public double? Sum(TNullableDoubleCollection current);
    public float Sum(TFloatCollection current);
    public float? Sum(TNullableFloatCollection current);
    public int Sum(TIntCollection current);
    public int? Sum(TNullableIntCollection current);
    public long Sum(TLongCollection current);
    public long? Sum(TNullableLongCollection current);
    public decimal Sum(TParentCollection current, Func<TParent, decimal> selector);
    public decimal? Sum(TParentCollection current, Func<TParent, decimal?> selector);
    public double Sum(TParentCollection current, Func<TParent, double> selector);
    public double? Sum(TParentCollection current, Func<TParent, double?> selector);
    public float Sum(TParentCollection current, Func<TParent, float> selector);
    public float? Sum(TParentCollection current, Func<TParent, float?> selector);
    public int Sum(TParentCollection current, Func<TParent, int> selector);
    public int? Sum(TParentCollection current, Func<TParent, int?> selector);
    public long Sum(TParentCollection current, Func<TParent, long> selector);
    public long? Sum(TParentCollection current, Func<TParent, long?> selector);
    #endregion

    #region Take
    public IEnumerable<TParent> Take(TParentCollection current, int count);
    public IEnumerable<TParent> Take(TParentCollection current, Range range);
    public IEnumerable<TParent> Take(TParentCollection current, LongRange range);
    #endregion

    #region TakeLast
    public IEnumerable<TParent> TakeLast(TParentCollection current, int count);
    public IEnumerable<TParent> TakeLast(TParentCollection current, long count);
    #endregion

    #region TakeWhile
    public IEnumerable<TParent> TakeWhile(TParentCollection current, Func<TParent, bool> predicate);
    public IEnumerable<TParent> TakeWhile(TParentCollection current, Func<TParent, long, bool> predicate);
    #endregion

    #region ToArray
    public TParent[] ToArray(TParentCollection current);
    #endregion

    #region ToImmutable
    public ImmutableArray<TParent> ToImmutableArray(TParentCollection current);
    public ImmutableHashSet<TParent> ToImmutableHashSet(TParentCollection current);
    public ImmutableHashSet<TParent> ToImmutableHashSet(TParentCollection current,
                                                        IEqualityComparer<TParent>? comparer);
    public ImmutableList<TParent> ToImmutableList(TParentCollection current);
    #endregion

    #region ToList
    public List<TParent> ToList(TParentCollection current);
    #endregion

    #region ToLookup
    public ILookup<TParent2, TParent> ToLookup(TParentCollection current, Func<TParent, TParent2> keySelector);
    public ILookup<TParent2, TParent> ToLookup(TParentCollection current,
                                               Func<TParent, TParent2> keySelector,
                                               IEqualityComparer<TParent2>? comparer);
    public ILookup<TParent2, TOtherElement> ToLookup(TParentCollection current,
                                                     Func<TParent, TParent2> keySelector,
                                                     Func<TParent, TOtherElement> elementSelector);

    public ILookup<TParent2, TOtherElement> ToLookup(TParentCollection current,
                                                     Func<TParent, TParent2> keySelector,
                                                     Func<TParent, TOtherElement> elementSelector,
                                                     IEqualityComparer<TParent2>? comparer);
    #endregion

    #region TryGetNonEnumeratedCount
    public bool TryGetNonEnumeratedCount(TParentCollection current, out int count);
    #endregion

    #region Union
    public IEnumerable<TParent> Union(IEnumerable<TParent> current, TChildCollection other);
    public IEnumerable<TParent> Union(IEnumerable<TParent> current, TChildCollection other,
                                      IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Union(TParentCollection current, IEnumerable<TParent> other);
    public IEnumerable<TParent> Union(TParentCollection current, IEnumerable<TParent> other,
                                      IEqualityComparer<TParent>? comparer);
    public IEnumerable<TParent> Union(TParentCollection current, TChildCollection other);
    public IEnumerable<TParent> Union(TParentCollection current, TChildCollection other,
                                      IEqualityComparer<TParent>? comparer);
    #endregion

    #region UnionBy
    public IEnumerable<TParent> UnionBy(IEnumerable<TParent> current, TChildCollection other,
                                        Func<TParent, TOtherElement> keySelector);
    public IEnumerable<TParent> UnionBy(IEnumerable<TParent> current, TChildCollection other,
                                        Func<TParent, TOtherElement> keySelector,
                                        IEqualityComparer<TOtherElement>? comparer);
    public IEnumerable<TParent> UnionBy(TParentCollection current, IEnumerable<TParent> other,
                                        Func<TParent, TOtherElement> keySelector);
    public IEnumerable<TParent> UnionBy(TParentCollection current, IEnumerable<TParent> other,
                                        Func<TParent, TOtherElement> keySelector,
                                        IEqualityComparer<TOtherElement>? comparer);
    public IEnumerable<TParent> UnionBy(TParentCollection current, TChildCollection other,
                                        Func<TParent, TOtherElement> keySelector);
    public IEnumerable<TParent> UnionBy(TParentCollection current, TChildCollection other,
                                        Func<TParent, TOtherElement> keySelector,
                                        IEqualityComparer<TOtherElement>? comparer);
    #endregion

    #region Where
    public IEnumerable<TParent> Where(TParentCollection current, Func<TParent, bool> predicate);
    public IEnumerable<TParent> Where(TParentCollection current, Func<TParent, long, bool> predicate);
    #endregion

    #region Zip
    public IEnumerable<(TParent, TParent2)> Zip(IEnumerable<TParent> first, TParent2Collection second);
    public IEnumerable<(TParent, TParent2)> Zip(TParentCollection first, IEnumerable<TParent2> second);
    public IEnumerable<(TParent, TParent2)> Zip(TParentCollection first, TParent2Collection second);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first,
                                                               TParent2Collection second,
                                                               IEnumerable<TOtherElement> third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(TParentCollection first,
                                                               IEnumerable<TParent2> second,
                                                               IEnumerable<TOtherElement> third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(TParentCollection first,
                                                               TParent2Collection second,
                                                               IEnumerable<TOtherElement> third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first,
                                                               IEnumerable<TParent2> second,
                                                               TOtherElementCollection third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first,
                                                               TParent2Collection second,
                                                               TOtherElementCollection third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(TParentCollection first,
                                                               IEnumerable<TParent2> second,
                                                               TOtherElementCollection third);
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(TParentCollection first,
                                                               TParent2Collection second,
                                                               TOtherElementCollection third);

    public IEnumerable<TOtherElement> Zip(IEnumerable<TParent> first, TChild2Collection second,
                                          Func<TParent, TParent2, TOtherElement> selector);
    public IEnumerable<TOtherElement> Zip(TParentCollection first, IEnumerable<TParent2> second,
                                          Func<TParent, TParent2, TOtherElement> selector);
    public IEnumerable<TOtherElement> Zip(TParentCollection first, TChild2Collection second,
                                          Func<TParent, TParent2, TOtherElement> selector);
    #endregion
}
