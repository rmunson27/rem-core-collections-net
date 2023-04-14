using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

public sealed class ReadOnlyArrayLinqImplementation<TParent, TChild, TParent2, TChild2, TOtherElement, TResult>
    : ILinqImplementation<ReadOnlyArray<TParent>, TParent, ReadOnlyArraySlice<TParent>,
                          ReadOnlyArray<TChild>, TChild,
                          ReadOnlyArray<TParent2>, TParent2,
                          ReadOnlyArray<TChild2>, TChild2,
                          ReadOnlyArray<TOtherElement>, TOtherElement,
                          TResult,
                          ReadOnlyArray<decimal>, ReadOnlyArray<decimal?>,
                          ReadOnlyArray<double>, ReadOnlyArray<double?>,
                          ReadOnlyArray<float>, ReadOnlyArray<float?>,
                          ReadOnlyArray<int>, ReadOnlyArray<int?>,
                          ReadOnlyArray<long>, ReadOnlyArray<long?>>
where TChild : TParent
where TChild2 : TParent2
{
    public TParent Aggregate(ReadOnlyArray<TParent> current, Func<TParent, TParent, TParent> func) => current.Aggregate(func);

    public TParent2 Aggregate(ReadOnlyArray<TParent> current, TParent2 seed, Func<TParent2, TParent, TParent2> func) => current.Aggregate(seed, func);

    public TResult Aggregate(ReadOnlyArray<TParent> current, TParent2 seed, Func<TParent2, TParent, TParent2> func, Func<TParent2, TResult> resultSelector)
        => current.Aggregate(seed, func, resultSelector);

    public bool All(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.All(predicate);

    public bool Any(ReadOnlyArray<TParent> current) => current.Any();

    public bool Any(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.Any(predicate);

    public IEnumerable<TParent> Append(ReadOnlyArray<TParent> current, TParent value) => current.Append(value);

    public decimal Average(ReadOnlyArray<decimal> current) => current.Average();

    public decimal? Average(ReadOnlyArray<decimal?> current) => current.Average();

    public double Average(ReadOnlyArray<double> current) => current.Average();

    public double? Average(ReadOnlyArray<double?> current) => current.Average();

    public float Average(ReadOnlyArray<float> current) => current.Average();

    public float? Average(ReadOnlyArray<float?> current) => current.Average();

    public double Average(ReadOnlyArray<int> current) => current.Average();

    public double? Average(ReadOnlyArray<int?> current) => current.Average();

    public double Average(ReadOnlyArray<long> current) => current.Average();

    public double? Average(ReadOnlyArray<long?> current) => current.Average();

    public decimal Average(ReadOnlyArray<TParent> current, Func<TParent, decimal> selector) => current.Average(selector);

    public decimal? Average(ReadOnlyArray<TParent> current, Func<TParent, decimal?> selector) => current.Average(selector);

    public double Average(ReadOnlyArray<TParent> current, Func<TParent, double> selector) => current.Average(selector);

    public double? Average(ReadOnlyArray<TParent> current, Func<TParent, double?> selector) => current.Average(selector);

    public float Average(ReadOnlyArray<TParent> current, Func<TParent, float> selector) => current.Average(selector);

    public float? Average(ReadOnlyArray<TParent> current, Func<TParent, float?> selector) => current.Average(selector);

    public double Average(ReadOnlyArray<TParent> current, Func<TParent, int> selector) => current.Average(selector);

    public double? Average(ReadOnlyArray<TParent> current, Func<TParent, int?> selector) => current.Average(selector);

    public double Average(ReadOnlyArray<TParent> current, Func<TParent, long> selector) => current.Average(selector);

    public double? Average(ReadOnlyArray<TParent> current, Func<TParent, long?> selector) => current.Average(selector);

    public IEnumerable<ReadOnlyArraySlice<TParent>> Chunk(ReadOnlyArray<TParent> current, int size) => current.Chunk(size);

    public IEnumerable<TParent> Concat(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other) => current.Concat(other);

    public IEnumerable<TParent> Concat(IEnumerable<TParent> current, ReadOnlyArray<TChild> other) => current.Concat(other);

    public IEnumerable<TParent> Concat(ReadOnlyArray<TParent> first, IEnumerable<TParent> second) => first.Concat(second);

    public bool Contains(ReadOnlyArray<TParent> current, TParent value) => current.Contains(value);

    public bool Contains(ReadOnlyArray<TParent> current, TParent value, IEqualityComparer<TParent>? comparer) => current.Contains(value, comparer);

    public int Count(ReadOnlyArray<TParent> current) => current.Count();

    public int Count(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.Count(predicate);

    public IEnumerable<TParent?> DefaultIfEmpty(ReadOnlyArray<TParent> current) => current.DefaultIfEmpty();

    public IEnumerable<TParent> DefaultIfEmpty(ReadOnlyArray<TParent> current, TParent defaultValue)
        => current.DefaultIfEmpty(defaultValue);

    public IEnumerable<TParent> Distinct(ReadOnlyArray<TParent> current) => current.Distinct();

    public IEnumerable<TParent> Distinct(ReadOnlyArray<TParent> current, IEqualityComparer<TParent>? comparer)
        => current.Distinct(comparer);

    public IEnumerable<TParent> DistinctBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector)
        => current.DistinctBy(keySelector);

    public IEnumerable<TParent> DistinctBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.DistinctBy(keySelector, comparer);

    public TParent ElementAt(ReadOnlyArray<TParent> current, int index) => current.ElementAt(index);

    public TParent ElementAt(ReadOnlyArray<TParent> current, Index index) => current.ElementAt(index);

    public TParent ElementAt(ReadOnlyArray<TParent> current, LongIndex index) => current.ElementAt(index);

    public TParent? ElementAtOrDefault(ReadOnlyArray<TParent> current, int index) => current.ElementAtOrDefault(index);

    public TParent? ElementAtOrDefault(ReadOnlyArray<TParent> current, Index index) => current.ElementAtOrDefault(index);
    public TParent? ElementAtOrDefault(ReadOnlyArray<TParent> current, LongIndex index) => current.ElementAtOrDefault(index);

    public IEnumerable<TParent> Except(ReadOnlyArray<TParent> first, IEnumerable<TParent> second) => first.Except(second);

    public IEnumerable<TParent> Except(ReadOnlyArray<TParent> first, IEnumerable<TParent> second, IEqualityComparer<TParent>? comparer)
        => first.Except(second, comparer);

    public IEnumerable<TParent> Except(IEnumerable<TParent> first, ReadOnlyArray<TChild> second) => first.Except(second);

    public IEnumerable<TParent> Except(IEnumerable<TParent> first, ReadOnlyArray<TChild> second, IEqualityComparer<TParent>? comparer)
        => first.Except(second, comparer);

    public IEnumerable<TParent> Except(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild> second) => first.Except(second);

    public IEnumerable<TParent> Except(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild> second, IEqualityComparer<TParent>? comparer)
        => first.Except(second, comparer);

    public IEnumerable<TParent> ExceptBy(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild2> keys, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.ExceptBy(keys, keySelector, comparer);

    public IEnumerable<TParent> ExceptBy(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild2> keys, Func<TParent, TParent2> keySelector)
        => current.ExceptBy(keys, keySelector);

    public IEnumerable<TParent> ExceptBy(IEnumerable<TParent> current, ReadOnlyArray<TChild2> keys, Func<TParent, TParent2> keySelector)
        => current.ExceptBy(keys, keySelector);

    public IEnumerable<TParent> ExceptBy(IEnumerable<TParent> current, ReadOnlyArray<TChild2> keys, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.ExceptBy(keys, keySelector, comparer);

    public IEnumerable<TParent> ExceptBy(ReadOnlyArray<TParent> current, IEnumerable<TParent2> keys, Func<TParent, TParent2> keySelector)
        => current.ExceptBy(keys, keySelector);

    public IEnumerable<TParent> ExceptBy(ReadOnlyArray<TParent> current, IEnumerable<TParent2> keys, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.ExceptBy(keys, keySelector, comparer);

    public TParent First(ReadOnlyArray<TParent> current) => current.First();
    public TParent First(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.First(predicate);

    public TParent? FirstOrDefault(ReadOnlyArray<TParent> current) => current.FirstOrDefault();

    public TParent? FirstOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.FirstOrDefault(predicate);

    public TParent FirstOrDefault(ReadOnlyArray<TParent> current, TParent defaultValue) => current.FirstOrDefault(defaultValue);

    public TParent FirstOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate, TParent defaultValue)
        => current.FirstOrDefault(predicate, defaultValue);

    public IEnumerable<TResult> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector, Func<TParent2, IEnumerable<TOtherElement>, TResult> resultSelector)
        => current.GroupBy(keySelector, elementSelector, resultSelector);

    public IEnumerable<TResult> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector, Func<TParent2, IEnumerable<TOtherElement>, TResult> resultSelector, IEqualityComparer<TParent2>? comparer)
        => current.GroupBy(keySelector, elementSelector, resultSelector, comparer);

    public IEnumerable<IGrouping<TParent2, TParent>> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector)
        => current.GroupBy(keySelector);

    public IEnumerable<IGrouping<TParent2, TParent>> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.GroupBy(keySelector, comparer);

    public IEnumerable<IGrouping<TParent2, TOtherElement>> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector)
        => current.GroupBy(keySelector, elementSelector);

    public IEnumerable<IGrouping<TParent2, TOtherElement>> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector, IEqualityComparer<TParent2>? comparer)
        => current.GroupBy(keySelector, elementSelector, comparer);

    public IEnumerable<TResult> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent2, IEnumerable<TParent>, TResult> resultSelector)
        => current.GroupBy(keySelector, resultSelector);

    public IEnumerable<TResult> GroupBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent2, IEnumerable<TParent>, TResult> resultSelector, IEqualityComparer<TParent2>? comparer)
        => current.GroupBy(keySelector, resultSelector, comparer);

    public IEnumerable<TResult> GroupJoin(ReadOnlyArray<TParent> outer, IEnumerable<TParent2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> GroupJoin(ReadOnlyArray<TParent> outer, IEnumerable<TParent2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public IEnumerable<TResult> GroupJoin(IEnumerable<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> GroupJoin(IEnumerable<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public IEnumerable<TResult> GroupJoin(ReadOnlyArray<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> GroupJoin(ReadOnlyArray<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, IEnumerable<TParent2>, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public IEnumerable<TParent> Intersect(IEnumerable<TParent> first, ReadOnlyArray<TChild> second)
        => first.Intersect(second);

    public IEnumerable<TParent> Intersect(IEnumerable<TParent> first, ReadOnlyArray<TChild> second, IEqualityComparer<TParent>? comparer)
        => first.Intersect(second, comparer);

    public IEnumerable<TParent> Intersect(ReadOnlyArray<TParent> first, IEnumerable<TParent> second)
        => first.Intersect(second);

    public IEnumerable<TParent> Intersect(ReadOnlyArray<TParent> first, IEnumerable<TParent> second, IEqualityComparer<TParent>? comparer)
        => first.Intersect(second, comparer);

    public IEnumerable<TParent> Intersect(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild> second)
        => first.Intersect(second);

    public IEnumerable<TParent> Intersect(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild> second, IEqualityComparer<TParent>? comparer)
        => first.Intersect(second, comparer);

    public IEnumerable<TParent> IntersectBy(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second, Func<TParent, TParent2> keySelector)
        => first.IntersectBy(second, keySelector);

    public IEnumerable<TParent> IntersectBy(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => first.IntersectBy(second, keySelector, comparer);

    public IEnumerable<TParent> IntersectBy(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2> keySelector)
        => first.IntersectBy(second, keySelector);

    public IEnumerable<TParent> IntersectBy(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => first.IntersectBy(second, keySelector, comparer);

    public IEnumerable<TParent> IntersectBy(IEnumerable<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2> keySelector)
        => first.IntersectBy(second, keySelector);

    public IEnumerable<TParent> IntersectBy(IEnumerable<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => first.IntersectBy(second, keySelector, comparer);

    public IEnumerable<TResult> Join(ReadOnlyArray<TParent> outer, IEnumerable<TParent2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> Join(ReadOnlyArray<TParent> outer, IEnumerable<TParent2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public IEnumerable<TResult> Join(IEnumerable<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> Join(IEnumerable<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public IEnumerable<TResult> Join(ReadOnlyArray<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

    public IEnumerable<TResult> Join(ReadOnlyArray<TParent> outer, ReadOnlyArray<TChild2> inner, Func<TParent, TOtherElement> outerKeySelector, Func<TParent2, TOtherElement> innerKeySelector, Func<TParent, TParent2, TResult> resultSelector, IEqualityComparer<TOtherElement>? comparer)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

    public TParent Last(ReadOnlyArray<TParent> current) => current.Last();

    public TParent Last(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.Last(predicate);

    public TParent? LastOrDefault(ReadOnlyArray<TParent> current) => current.LastOrDefault();

    public TParent? LastOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.LastOrDefault(predicate);

    public TParent LastOrDefault(ReadOnlyArray<TParent> current, TParent defaultValue) => current.LastOrDefault(defaultValue);

    public TParent LastOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate, TParent defaultValue)
        => current.LastOrDefault(predicate, defaultValue);

    public long LongCount(ReadOnlyArray<TParent> current) => current.LongCount();

    public long LongCount(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.LongCount(predicate);

    public decimal Max(ReadOnlyArray<decimal> current) => current.Max();

    public decimal? Max(ReadOnlyArray<decimal?> current) => current.Max();

    public double Max(ReadOnlyArray<double> current) => current.Max();

    public double? Max(ReadOnlyArray<double?> current) => current.Max();

    public float Max(ReadOnlyArray<float> current) => current.Max();

    public float? Max(ReadOnlyArray<float?> current) => current.Max();

    public int Max(ReadOnlyArray<int> current) => current.Max();

    public int? Max(ReadOnlyArray<int?> current) => current.Max();

    public long Max(ReadOnlyArray<long> current) => current.Max();

    public long? Max(ReadOnlyArray<long?> current) => current.Max();

    public TParent? Max(ReadOnlyArray<TParent> current) => current.Max();

    public TParent2? Max(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector) => current.Max(selector);

    public TParent? Max(ReadOnlyArray<TParent> current, IComparer<TParent>? comparer) => current.Max(comparer);

    public decimal Max(ReadOnlyArray<TParent> current, Func<TParent, decimal> selector) => current.Max(selector);

    public decimal? Max(ReadOnlyArray<TParent> current, Func<TParent, decimal?> selector) => current.Max(selector);

    public double Max(ReadOnlyArray<TParent> current, Func<TParent, double> selector) => current.Max(selector);

    public double? Max(ReadOnlyArray<TParent> current, Func<TParent, double?> selector) => current.Max(selector);

    public float Max(ReadOnlyArray<TParent> current, Func<TParent, float> selector) => current.Max(selector);

    public float? Max(ReadOnlyArray<TParent> current, Func<TParent, float?> selector) => current.Max(selector);

    public int Max(ReadOnlyArray<TParent> current, Func<TParent, int> selector) => current.Max(selector);

    public int? Max(ReadOnlyArray<TParent> current, Func<TParent, int?> selector) => current.Max(selector);

    public long Max(ReadOnlyArray<TParent> current, Func<TParent, long> selector) => current.Max(selector);

    public long? Max(ReadOnlyArray<TParent> current, Func<TParent, long?> selector) => current.Max(selector);

    public TParent? MaxBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector) => current.MaxBy(selector);

    public TParent? MaxBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector, IComparer<TParent2> comparer) 
        => current.MaxBy(selector, comparer);

    public decimal Min(ReadOnlyArray<decimal> current) => current.Min();

    public decimal? Min(ReadOnlyArray<decimal?> current) => current.Min();

    public double Min(ReadOnlyArray<double> current) => current.Min();

    public double? Min(ReadOnlyArray<double?> current) => current.Min();

    public float Min(ReadOnlyArray<float> current) => current.Min();

    public float? Min(ReadOnlyArray<float?> current) => current.Min();

    public int Min(ReadOnlyArray<int> current) => current.Min();

    public int? Min(ReadOnlyArray<int?> current) => current.Min();

    public long Min(ReadOnlyArray<long> current) => current.Min();

    public long? Min(ReadOnlyArray<long?> current) => current.Min();

    public TParent? Min(ReadOnlyArray<TParent> current) => current.Min();

    public TParent2? Min(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector) => current.Min(selector);

    public TParent? Min(ReadOnlyArray<TParent> current, IComparer<TParent>? comparer) => current.Min(comparer);

    public decimal Min(ReadOnlyArray<TParent> current, Func<TParent, decimal> selector) => current.Min(selector);

    public decimal? Min(ReadOnlyArray<TParent> current, Func<TParent, decimal?> selector) => current.Min(selector);

    public double Min(ReadOnlyArray<TParent> current, Func<TParent, double> selector) => current.Min(selector);

    public double? Min(ReadOnlyArray<TParent> current, Func<TParent, double?> selector) => current.Min(selector);

    public float Min(ReadOnlyArray<TParent> current, Func<TParent, float> selector) => current.Min(selector);

    public float? Min(ReadOnlyArray<TParent> current, Func<TParent, float?> selector) => current.Min(selector);

    public int Min(ReadOnlyArray<TParent> current, Func<TParent, int> selector) => current.Min(selector);

    public int? Min(ReadOnlyArray<TParent> current, Func<TParent, int?> selector) => current.Min(selector);

    public long Min(ReadOnlyArray<TParent> current, Func<TParent, long> selector) => current.Min(selector);

    public long? Min(ReadOnlyArray<TParent> current, Func<TParent, long?> selector) => current.Min(selector);

    public TParent? MinBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector) => current.MinBy(selector);

    public TParent? MinBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector, IComparer<TParent2> comparer)
        => current.MinBy(selector, comparer);

    public IOrderedEnumerable<TParent> Order(ReadOnlyArray<TParent> current) => current.Order();

    public IOrderedEnumerable<TParent> Order(ReadOnlyArray<TParent> current, IComparer<TParent>? comparer) => current.Order(comparer);
    public IOrderedEnumerable<TParent> OrderBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector)
        => current.OrderBy(selector);

    public IOrderedEnumerable<TParent> OrderBy(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector, IComparer<TParent2>? comparer)
        => current.OrderBy(selector, comparer);

    public IOrderedEnumerable<TParent> OrderByDescending(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector)
        => current.OrderByDescending(selector);

    public IOrderedEnumerable<TParent> OrderByDescending(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector, IComparer<TParent2>? comparer)
        => current.OrderByDescending(selector, comparer);

    public IOrderedEnumerable<TParent> OrderDescending(ReadOnlyArray<TParent> current) => current.OrderDescending();
    public IOrderedEnumerable<TParent> OrderDescending(ReadOnlyArray<TParent> current, IComparer<TParent>? comparer) => current.OrderDescending(comparer);

    public IEnumerable<TParent> Prepend(ReadOnlyArray<TParent> current, TParent value) => current.Prepend(value);

    public IEnumerable<TParent> Reverse(ReadOnlyArray<TParent> current) => current.Reverse();

    public IEnumerable<TParent2> Select(ReadOnlyArray<TParent> current, Func<TParent, TParent2> selector) => current.Select(selector);

    public IEnumerable<TParent2> Select(ReadOnlyArray<TParent> current, Func<TParent, long, TParent2> selector) => current.Select(selector);

    public IEnumerable<TParent2> SelectMany(ReadOnlyArray<TParent> current, Func<TParent, IEnumerable<TParent2>> selector) => current.SelectMany(selector);

    public IEnumerable<TParent2> SelectMany(ReadOnlyArray<TParent> current, Func<TParent, long, IEnumerable<TParent2>> selector) => current.SelectMany(selector);

    public bool SequenceEqual(IEnumerable<TParent> current, ReadOnlyArray<TChild> other) => current.SequenceEqual(other);
    public bool SequenceEqual(IEnumerable<TParent> current, ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? comparer)
        => current.SequenceEqual(other, comparer);
    public bool SequenceEqual(ReadOnlyArray<TParent> current, IEnumerable<TParent> other) => current.SequenceEqual(other);

    public bool SequenceEqual(ReadOnlyArray<TParent> current, IEnumerable<TParent> other, IEqualityComparer<TParent>? comparer)
        => current.SequenceEqual(other, comparer);

    public bool SequenceEqual(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other) => current.SequenceEqual(other);

    public bool SequenceEqual(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? comparer) => current.SequenceEqual(other, comparer);

    public TParent Single(ReadOnlyArray<TParent> current) => current.Single();
    public TParent Single(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.Single(predicate);

    public TParent? SingleOrDefault(ReadOnlyArray<TParent> current) => current.SingleOrDefault();

    public TParent? SingleOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.SingleOrDefault(predicate);

    public TParent SingleOrDefault(ReadOnlyArray<TParent> current, TParent defaultValue) => current.SingleOrDefault(defaultValue);

    public TParent SingleOrDefault(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate, TParent defaultValue)
        => current.SingleOrDefault(predicate, defaultValue);

    public IEnumerable<TParent> Skip(ReadOnlyArray<TParent> current, int count) => current.Skip(count);

    public IEnumerable<TParent> Skip(ReadOnlyArray<TParent> current, long count) => current.Skip(count);

    public IEnumerable<TParent> SkipLast(ReadOnlyArray<TParent> current, int count) => current.SkipLast(count);

    public IEnumerable<TParent> SkipLast(ReadOnlyArray<TParent> current, long count) => current.SkipLast(count);

    public IEnumerable<TParent> SkipWhile(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.SkipWhile(predicate);

    public IEnumerable<TParent> SkipWhile(ReadOnlyArray<TParent> current, Func<TParent, long, bool> predicate) => current.SkipWhile(predicate);

    public decimal Sum(ReadOnlyArray<decimal> current) => current.Sum();

    public decimal? Sum(ReadOnlyArray<decimal?> current) => current.Sum();

    public double Sum(ReadOnlyArray<double> current) => current.Sum();

    public double? Sum(ReadOnlyArray<double?> current) => current.Sum();

    public float Sum(ReadOnlyArray<float> current) => current.Sum();

    public float? Sum(ReadOnlyArray<float?> current) => current.Sum();

    public int Sum(ReadOnlyArray<int> current) => current.Sum();

    public int? Sum(ReadOnlyArray<int?> current) => current.Sum();

    public long Sum(ReadOnlyArray<long> current) => current.Sum();

    public long? Sum(ReadOnlyArray<long?> current) => current.Sum();

    public decimal Sum(ReadOnlyArray<TParent> current, Func<TParent, decimal> selector) => current.Sum(selector);

    public decimal? Sum(ReadOnlyArray<TParent> current, Func<TParent, decimal?> selector) => current.Sum(selector);

    public double Sum(ReadOnlyArray<TParent> current, Func<TParent, double> selector) => current.Sum(selector);

    public double? Sum(ReadOnlyArray<TParent> current, Func<TParent, double?> selector) => current.Sum(selector);

    public float Sum(ReadOnlyArray<TParent> current, Func<TParent, float> selector) => current.Sum(selector);

    public float? Sum(ReadOnlyArray<TParent> current, Func<TParent, float?> selector) => current.Sum(selector);

    public int Sum(ReadOnlyArray<TParent> current, Func<TParent, int> selector) => current.Sum(selector);

    public int? Sum(ReadOnlyArray<TParent> current, Func<TParent, int?> selector) => current.Sum(selector);

    public long Sum(ReadOnlyArray<TParent> current, Func<TParent, long> selector) => current.Sum(selector);

    public long? Sum(ReadOnlyArray<TParent> current, Func<TParent, long?> selector) => current.Sum(selector);

    public IEnumerable<TParent> Take(ReadOnlyArray<TParent> current, int count) => current.Take(count);

    public IEnumerable<TParent> Take(ReadOnlyArray<TParent> current, long count) => current.Take(count);

    public IEnumerable<TParent> Take(ReadOnlyArray<TParent> current, Range range) => current.Take(range);

    public IEnumerable<TParent> Take(ReadOnlyArray<TParent> current, LongRange range) => current.Take(range);

    public IEnumerable<TParent> TakeLast(ReadOnlyArray<TParent> current, int count) => current.TakeLast(count);

    public IEnumerable<TParent> TakeLast(ReadOnlyArray<TParent> current, long count) => current.TakeLast(count);

    public IEnumerable<TParent> TakeWhile(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.TakeWhile(predicate);

    public IEnumerable<TParent> TakeWhile(ReadOnlyArray<TParent> current, Func<TParent, long, bool> predicate) => current.TakeWhile(predicate);

    public TParent[] ToArray(ReadOnlyArray<TParent> current) => current.ToArray();

    public ImmutableArray<TParent> ToImmutableArray(ReadOnlyArray<TParent> current) => current.ToImmutableArray();

    public ImmutableHashSet<TParent> ToImmutableHashSet(ReadOnlyArray<TParent> current) => current.ToImmutableHashSet();

    public ImmutableHashSet<TParent> ToImmutableHashSet(ReadOnlyArray<TParent> current, IEqualityComparer<TParent>? comparer)
        => current.ToImmutableHashSet(comparer);

    public ImmutableList<TParent> ToImmutableList(ReadOnlyArray<TParent> current) => current.ToImmutableList();

    public List<TParent> ToList(ReadOnlyArray<TParent> current) => current.ToList();

    public ILookup<TParent2, TParent> ToLookup(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector) => current.ToLookup(keySelector);
    public ILookup<TParent2, TParent> ToLookup(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, IEqualityComparer<TParent2>? comparer)
        => current.ToLookup(keySelector, comparer);

    public ILookup<TParent2, TOtherElement> ToLookup(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector)
        => current.ToLookup(keySelector, elementSelector);

    public ILookup<TParent2, TOtherElement> ToLookup(ReadOnlyArray<TParent> current, Func<TParent, TParent2> keySelector, Func<TParent, TOtherElement> elementSelector, IEqualityComparer<TParent2>? comparer)
        => current.ToLookup(keySelector, elementSelector, comparer);

    public bool TryGetNonEnumeratedCount(ReadOnlyArray<TParent> current, out int count) => current.TryGetNonEnumeratedCount(out count);

    public IEnumerable<TParent> Union(IEnumerable<TParent> current, ReadOnlyArray<TChild> other) => current.Union(other);

    public IEnumerable<TParent> Union(IEnumerable<TParent> current, ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? comparer) => current.Union(other, comparer);

    public IEnumerable<TParent> Union(ReadOnlyArray<TParent> current, IEnumerable<TParent> other) => current.Union(other);
    public IEnumerable<TParent> Union(ReadOnlyArray<TParent> current, IEnumerable<TParent> other, IEqualityComparer<TParent>? comparer)
        => current.Union(other, comparer);
    public IEnumerable<TParent> Union(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other) => current.Union(other);

    public IEnumerable<TParent> Union(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other, IEqualityComparer<TParent>? comparer)
        => current.Union(other, comparer);

    public IEnumerable<TParent> UnionBy(IEnumerable<TParent> current, ReadOnlyArray<TChild> other, Func<TParent, TOtherElement> keySelector)
        => current.UnionBy(other, keySelector);

    public IEnumerable<TParent> UnionBy(IEnumerable<TParent> current, ReadOnlyArray<TChild> other, Func<TParent, TOtherElement> keySelector, IEqualityComparer<TOtherElement>? comparer)
        => ReadOnlyArray.UnionBy<TParent, TChild, TOtherElement>(current, other, keySelector, comparer);

    public IEnumerable<TParent> UnionBy(ReadOnlyArray<TParent> current, IEnumerable<TParent> other, Func<TParent, TOtherElement> keySelector)
        => current.UnionBy(other, keySelector);

    public IEnumerable<TParent> UnionBy(ReadOnlyArray<TParent> current, IEnumerable<TParent> other, Func<TParent, TOtherElement> keySelector, IEqualityComparer<TOtherElement>? comparer)
        => current.UnionBy(other, keySelector, comparer);

    public IEnumerable<TParent> UnionBy(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other, Func<TParent, TOtherElement> keySelector)
        => current.UnionBy(other, keySelector);

    public IEnumerable<TParent> UnionBy(ReadOnlyArray<TParent> current, ReadOnlyArray<TChild> other, Func<TParent, TOtherElement> keySelector, IEqualityComparer<TOtherElement>? comparer)
        => current.UnionBy(other, keySelector, comparer);

    public IEnumerable<TParent> Where(ReadOnlyArray<TParent> current, Func<TParent, bool> predicate) => current.Where(predicate);

    public IEnumerable<TParent> Where(ReadOnlyArray<TParent> current, Func<TParent, long, bool> predicate) => current.Where(predicate);

    public IEnumerable<(TParent, TParent2)> Zip(IEnumerable<TParent> first, ReadOnlyArray<TParent2> second) => first.Zip(second);
    public IEnumerable<(TParent, TParent2)> Zip(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second) => first.Zip(second);

    public IEnumerable<(TParent, TParent2)> Zip(ReadOnlyArray<TParent> first, ReadOnlyArray<TParent2> second) => first.Zip(second);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first, ReadOnlyArray<TParent2> second, IEnumerable<TOtherElement> third)
        => first.Zip(second, third);
    
    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second, IEnumerable<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(ReadOnlyArray<TParent> first, ReadOnlyArray<TParent2> second, IEnumerable<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first, IEnumerable<TParent2> second, ReadOnlyArray<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(IEnumerable<TParent> first, ReadOnlyArray<TParent2> second, ReadOnlyArray<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second, ReadOnlyArray<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<(TParent, TParent2, TOtherElement)> Zip(ReadOnlyArray<TParent> first, ReadOnlyArray<TParent2> second, ReadOnlyArray<TOtherElement> third)
        => first.Zip(second, third);

    public IEnumerable<TOtherElement> Zip(IEnumerable<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2, TOtherElement> selector)
        => first.Zip(second, selector);

    public IEnumerable<TOtherElement> Zip(ReadOnlyArray<TParent> first, IEnumerable<TParent2> second, Func<TParent, TParent2, TOtherElement> selector)
        => first.Zip(second, selector);

    public IEnumerable<TOtherElement> Zip(ReadOnlyArray<TParent> first, ReadOnlyArray<TChild2> second, Func<TParent, TParent2, TOtherElement> selector)
        => first.Zip(second, selector);
}
