using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

[TestClass]
public abstract class LinqImplementationTest<TNumber1Collection, TNumber1Chunk,
                                             TNumber1ACollection,
                                             TNumber2Collection,
                                             TNumber2ACollection,
                                             TNumber3Collection,
                                             TDecimalCollection, TNullableDecimalCollection,
                                             TDoubleCollection, TNullableDoubleCollection,
                                             TFloatCollection, TNullableFloatCollection,
                                             TIntCollection, TNullableIntCollection,
                                             TLongCollection, TNullableLongCollection>
where TNumber1Collection : IEnumerable<Number1> where TNumber1Chunk : IEnumerable<Number1>
where TNumber1ACollection : IEnumerable<Number1Child>
where TNumber2Collection : IEnumerable<Number2>
where TNumber2ACollection : IEnumerable<Number2Child>
where TNumber3Collection : IEnumerable<Number3>
where TDecimalCollection : IEnumerable<decimal> where TNullableDecimalCollection : IEnumerable<decimal?>
where TDoubleCollection : IEnumerable<double> where TNullableDoubleCollection : IEnumerable<double?>
where TFloatCollection : IEnumerable<float> where TNullableFloatCollection : IEnumerable<float?>
where TIntCollection : IEnumerable<int> where TNullableIntCollection : IEnumerable<int?>
where TLongCollection : IEnumerable<long> where TNullableLongCollection : IEnumerable<long?>
{
    #region Concrete
    #region Constants
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> that compares instances of <see cref="Number1"/> for
    /// equality by comparing their <see cref="Number1.Value"/> properties modulo 3.
    /// </summary>
    protected static readonly EqualMod9BucketMod3Comparer<Number1> Number1Mod9Comparer = new();

    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> that compares instances of <see cref="Number2"/> for
    /// equality by comparing their <see cref="Number2.Value"/> properties modulo 3.
    /// </summary>
    protected static readonly EqualMod9BucketMod3Comparer<Number2> Number2Mod9Comparer = new();

    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> that compares instances of <see cref="Number3"/> for
    /// equality by comparing their <see cref="Number3.Value"/> properties modulo 3.
    /// </summary>
    protected static readonly EqualMod9BucketMod3Comparer<Number3> Number3Mod9Comparer = new();

    /// <summary>
    /// An <see cref="IComparer{T}"/> that compares instances of <see cref="Number1"/> based on their absolute values.
    /// </summary>
    protected static readonly AbsoluteValueComparer<Number1> Number1AbsoluteValueComparer = new();

    /// <summary>
    /// An <see cref="IComparer{T}"/> that compares instances of <see cref="Number2"/> based on their absolute values.
    /// </summary>
    protected static readonly AbsoluteValueComparer<Number2> Number2AbsoluteValueComparer = new();

    protected static GroupingEqualityComparer<Number2, TValue> GroupingComparer<TValue>() => new();
    #endregion

    #region Properties
    protected ReferenceTestCollections<TNumber1Collection, Number1> Number1s { get; }
    protected ReferenceTestCollections<TNumber1ACollection, Number1Child> Number1Children { get; }
    protected ReferenceTestCollections<TNumber2Collection, Number2> Number2s { get; }
    protected ReferenceTestCollections<TNumber2ACollection, Number2Child> Number2Children { get; }
    protected ReferenceTestCollections<TNumber3Collection, Number3> Number3s { get; }

    protected TestCollections<TDecimalCollection, decimal> Decimals { get; }
    protected NullableTestCollections<TNullableDecimalCollection, decimal> NullableDecimals { get; }
    protected TestCollections<TDoubleCollection, double> Doubles { get; }
    protected NullableTestCollections<TNullableDoubleCollection, double> NullableDoubles { get; }
    protected TestCollections<TFloatCollection, float> Floats { get; }
    protected NullableTestCollections<TNullableFloatCollection, float> NullableFloats { get; }
    protected TestCollections<TIntCollection, int> Ints { get; }
    protected NullableTestCollections<TNullableIntCollection, int> NullableInts { get; }
    protected TestCollections<TLongCollection, long> Longs { get; }
    protected NullableTestCollections<TNullableLongCollection, long> NullableLongs { get; }
    #endregion

    #region Constructor
    protected LinqImplementationTest()
    {
        Number1s = new(FirstTen: NewNumber1s(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                       FirstHundred: NewNumber1s(from x in TestCollections.FirstHundredRange select new Number1Child(x)),
                       Empty: NewNumber1s(Enumerable.Empty<Number1Child>()),
                       FirstHundredNoFives: NewNumber1s(from x in TestCollections.FirstHundredNoFivesRange
                                                        select x is int i ? new Number1Child(i) : null),
                       AllNull: NewNumber1s(Enumerable.Repeat(default(Number1Child), 100)!));
        Number1Children = new(FirstTen: NewNumber1Children(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                              FirstHundred: NewNumber1Children(from x in TestCollections.FirstHundredRange
                                                               select new Number1Child(x)),
                              Empty: NewNumber1Children(Enumerable.Empty<Number1Child>()),
                              FirstHundredNoFives: NewNumber1Children(
                                                    from x in TestCollections.FirstHundredNoFivesRange
                                                    select x is int i ? new Number1Child(i) : null),
                              AllNull: NewNumber1Children(Enumerable.Repeat(default(Number1Child), 100)!));

        Number2s = new(FirstTen: NewNumber2s(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                       FirstHundred: NewNumber2s(from x in TestCollections.FirstHundredRange select new Number2Child(x)),
                       Empty: NewNumber2s(Enumerable.Empty<Number2Child>()),
                       FirstHundredNoFives: NewNumber2s(from x in TestCollections.FirstHundredNoFivesRange
                                                        select x is int i ? new Number2Child(i) : null),
                       AllNull: NewNumber2s(Enumerable.Repeat(default(Number2Child), 100)!));
        Number2Children = new(FirstTen: NewNumber2Children(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                              FirstHundred: NewNumber2Children(from x in TestCollections.FirstHundredRange
                                                               select new Number2Child(x)),
                              Empty: NewNumber2Children(Enumerable.Empty<Number2Child>()),
                              FirstHundredNoFives: NewNumber2Children(
                                                    from x in TestCollections.FirstHundredNoFivesRange
                                                    select x is int i ? new Number2Child(i) : null),
                              AllNull: NewNumber2Children(Enumerable.Repeat(default(Number2Child), 100)!));

        Number3s = new(FirstTen: NewNumber3s(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
                       FirstHundred: NewNumber3s(from x in TestCollections.FirstHundredRange select new Number3(x)),
                       Empty: NewNumber3s(Enumerable.Empty<Number3>()),
                       FirstHundredNoFives: NewNumber3s(from x in TestCollections.FirstHundredNoFivesRange
                                                        select x is int i ? new Number3(i) : null),
                       AllNull: NewNumber3s(Enumerable.Repeat(default(Number3), 100)!));

        Decimals = new(FirstTen: New(from x in TestCollections.FirstTenRange select (decimal)x),
                       FirstHundred: New(from x in TestCollections.FirstHundredRange select (decimal)x),
                       Empty: New(Enumerable.Empty<decimal>()));
        NullableDecimals
            = new(FirstTen: NewNullable(from x in TestCollections.FirstTenRange select (decimal?)x),
                  FirstHundredNoFives: NewNullable(from x in TestCollections.FirstHundredNoFivesRange
                                                   select (decimal?)x),
                  AllNull: NewNullable(Enumerable.Repeat(default(decimal?), 100)),
                  Empty: NewNullable(Enumerable.Empty<decimal?>()));

        Doubles = new(FirstTen: New(from x in TestCollections.FirstTenRange select (double)x),
                      FirstHundred: New(from x in TestCollections.FirstHundredRange select (double)x),
                      Empty: New(Enumerable.Empty<double>()));
        NullableDoubles
            = new(FirstTen: NewNullable(from x in TestCollections.FirstTenRange select (double?)x),
                  FirstHundredNoFives: NewNullable(from x in TestCollections.FirstHundredNoFivesRange
                                                   select (double?)x),
                  AllNull: NewNullable(Enumerable.Repeat(default(double?), 100)),
                  Empty: NewNullable(Enumerable.Empty<double?>()));

        Floats = new(FirstTen: New(from x in TestCollections.FirstTenRange select (float)x),
                     FirstHundred: New(from x in TestCollections.FirstHundredRange select (float)x),
                     Empty: New(Enumerable.Empty<float>()));
        NullableFloats
            = new(FirstTen: NewNullable(from x in TestCollections.FirstTenRange select (float?)x),
                  FirstHundredNoFives: NewNullable(from x in TestCollections.FirstHundredNoFivesRange
                                                   select (float?)x),
                  AllNull: NewNullable(Enumerable.Repeat(default(float?), 100)),
                  Empty: NewNullable(Enumerable.Empty<float?>()));

        Ints = new(FirstTen: New(TestCollections.FirstTenRange),
                   FirstHundred: New(TestCollections.FirstHundredRange),
                   Empty: New(Enumerable.Empty<int>()));
        NullableInts
            = new(FirstTen: NewNullable(from x in TestCollections.FirstTenRange select (int?)x),
                  FirstHundredNoFives: NewNullable(TestCollections.FirstHundredNoFivesRange),
                  AllNull: NewNullable(Enumerable.Repeat(default(int?), 100)),
                  Empty: NewNullable(Enumerable.Empty<int?>()));

        Longs = new(FirstTen: New(from x in TestCollections.FirstTenRange select (long)x),
                    FirstHundred: New(from x in TestCollections.FirstHundredRange select (long)x),
                    Empty: New(Enumerable.Empty<long>()));
        NullableLongs
            = new(FirstTen: NewNullable(from x in TestCollections.FirstTenRange select (long?)x),
                  FirstHundredNoFives: NewNullable(from x in TestCollections.FirstHundredNoFivesRange
                                                   select (long?)x),
                  AllNull: NewNullable(Enumerable.Repeat(default(long?), 100)),
                  Empty: NewNullable(Enumerable.Empty<long?>()));
    }
    #endregion

    #region Tests
    #region A
    /// <summary>
    /// Tests the aggregator methods.
    /// </summary>
    [TestMethod]
    public void TestAggregate()
    {
        Assert.AreEqual(new(TestCollections.FirstHundredSum),
                        Implementation.Aggregate(Number1s.FirstHundred, Number1.Add));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Aggregate(Number1s.Empty, Number1.Add));

        Assert.AreEqual(new Number2(TestCollections.FirstHundredSum),
                        Implementation.Aggregate(Number1s.FirstHundred, new(0), Number1.Add));
        Assert.AreEqual(new Number2(4), Implementation.Aggregate(Number1s.Empty, new(4), Number1.Add));

        Assert.AreEqual(TestCollections.FirstHundredSum,
                        Implementation.Aggregate(Number1s.FirstHundred, new(0), Number1.Add, n => n!.Value));
        Assert.AreEqual(4, Implementation.Aggregate(Number1s.Empty, new(4), Number1.Add, n => n!.Value));
    }

    /// <summary>
    /// Tests the "All" method.
    /// </summary>
    [TestMethod]
    public void TestAll()
    {
        Assert.IsTrue(Implementation.All(Number1s.FirstHundred, n => n!.Value >= 0));
        Assert.IsFalse(Implementation.All(Number1s.FirstHundred, n => n!.Value < 50));
        Assert.IsTrue(Implementation.All(Number1s.Empty, _ => false));
    }

    /// <summary>
    /// Tests the "Any" methods.
    /// </summary>
    [TestMethod]
    public void TestAny()
    {
        Assert.IsTrue(Implementation.Any(Number1s.FirstHundred));
        Assert.IsFalse(Implementation.Any(Number1s.Empty));

        Assert.IsTrue(Implementation.Any(Number1s.FirstHundred, n => n!.Value < 50));
        Assert.IsFalse(Implementation.Any(Number1s.FirstHundred, n => n!.Value < 0));
        Assert.IsFalse(Implementation.Any(Number1s.Empty, _ => true));
    }

    /// <summary>
    /// Tests the append method.
    /// </summary>
    [TestMethod]
    public void TestAppend()
    {
        Assert.That.AreSequenceEqual(TestCollections.FirstHundredRange.Append(1).Select(n => new Number1Child(n)),
                                      Implementation.Append(Number1s.FirstHundred, new Number1Child(1)));
    }

    /// <summary>
    /// Tests the "Average" methods.
    /// </summary>
    [TestMethod]
    public void TestAverage()
    {
        Assert.AreEqual(TestCollections.FirstHundredDecimalAverage, Implementation.Average(Decimals.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredDoubleAverage, Implementation.Average(Doubles.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredFloatAverage, Implementation.Average(Floats.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredDoubleAverage, Implementation.Average(Ints.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredDoubleAverage, Implementation.Average(Longs.FirstHundred));

        Assert.AreEqual(TestCollections.FirstHundredNoFivesDecimalAverage,
                        Implementation.Average(NullableDecimals.FirstHundredNoFives));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(NullableDoubles.FirstHundredNoFives));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesFloatAverage,
                        Implementation.Average(NullableFloats.FirstHundredNoFives));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(NullableInts.FirstHundredNoFives));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(NullableLongs.FirstHundredNoFives));

        Assert.IsNull(Implementation.Average(NullableDecimals.AllNull));
        Assert.IsNull(Implementation.Average(NullableDoubles.AllNull));
        Assert.IsNull(Implementation.Average(NullableFloats.AllNull));
        Assert.IsNull(Implementation.Average(NullableInts.AllNull));
        Assert.IsNull(Implementation.Average(NullableLongs.AllNull));

        Assert.AreEqual(TestCollections.FirstHundredNoFivesDecimalAverage,
                        Implementation.Average(Number1s.FirstHundredNoFives, n => (decimal?)n?.Value));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(Number1s.FirstHundredNoFives, n => (double?)n?.Value));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesFloatAverage,
                        Implementation.Average(Number1s.FirstHundredNoFives, n => (float?)n?.Value));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(Number1s.FirstHundredNoFives, n => (int?)n?.Value));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesDoubleAverage,
                        Implementation.Average(Number1s.FirstHundredNoFives, n => (long?)n?.Value));

        Assert.IsNull(Implementation.Average(Number1s.AllNull, n => (decimal?)n?.Value));
        Assert.IsNull(Implementation.Average(Number1s.AllNull, n => (double?)n?.Value));
        Assert.IsNull(Implementation.Average(Number1s.AllNull, n => (float?)n?.Value));
        Assert.IsNull(Implementation.Average(Number1s.AllNull, n => (int?)n?.Value));
        Assert.IsNull(Implementation.Average(Number1s.AllNull, n => (long?)n?.Value));
    }
    #endregion

    #region C
    /// <summary>
    /// Tests the "Chunk" method.
    /// </summary>
    [TestMethod]
    public void TestChunk()
    {
        // Chunk size is greater than length of collection
        Assert.That.AreSequenceEqual(TestCollections.FirstHundredRange
                                        .Chunk(200)
                                        .Select(c => c.Select(v => new Number1Child(v))),
                                     Implementation.Chunk(Number1s.FirstHundred, 200).Select(c => c.AsEnumerable()),
                                     Enumerables.SequenceEqualityComparer<Number1>());

        // Chunk size is length of collection
        Assert.That.AreSequenceEqual(TestCollections.FirstHundredRange
                                        .Chunk(100)
                                        .Select(c => c.Select(v => new Number1Child(v))),
                                     Implementation.Chunk(Number1s.FirstHundred, 100).Select(c => c.AsEnumerable()),
                                     Enumerables.SequenceEqualityComparer<Number1>());

        // Length is a multiple of chunk size
        Assert.That.AreSequenceEqual(TestCollections.FirstHundredRange
                                        .Chunk(2)
                                        .Select(c => c.Select(v => new Number1Child(v))),
                                     Implementation.Chunk(Number1s.FirstHundred, 2).Select(c => c.AsEnumerable()),
                                     Enumerables.SequenceEqualityComparer<Number1>());

        // Length is not a multiple of chunk size
        Assert.That.AreSequenceEqual(TestCollections.FirstHundredRange
                                        .Chunk(3)
                                        .Select(c => c.Select(v => new Number1Child(v))),
                                     Implementation.Chunk(Number1s.FirstHundred, 3).Select(c => c.AsEnumerable()),
                                     Enumerables.SequenceEqualityComparer<Number1>());

        // Chunk size is too small
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Implementation.Chunk(Number1s.FirstHundred, 0));
    }

    /// <summary>
    /// Tests the "Concat" methods.
    /// </summary>
    [TestMethod]
    public void TestConcat()
    {
        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Concat(Number1Children.FirstHundred),
                                      Implementation.Concat(Number1s.FirstHundred, Number1Children.FirstHundred));

        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Concat(Number1Children.FirstHundred),
                                      Implementation.Concat(Number1s.FirstHundred,
                                                            Number1Children.FirstHundred as IEnumerable<Number1>));

        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Concat(Number1Children.FirstHundred),
                                      Implementation.Concat(Number1s.FirstHundred as IEnumerable<Number1>,
                                                            Number1Children.FirstHundred));
    }

    /// <summary>
    /// Tests the "Contains" methods.
    /// </summary>
    [TestMethod]
    public void TestContains()
    {

        Assert.IsTrue(Implementation.Contains(Number1s.FirstHundred, new Number1Child(4)));
        Assert.IsFalse(Implementation.Contains(Number1s.FirstHundred, new Number1Child(400)));

        // Numbers are too large, but equal something in the collection mod 9
        Assert.IsFalse(Implementation.Contains(Number1s.FirstHundred, new Number1Child(500))); // Control
        Assert.IsFalse(Implementation.Contains(Number1s.FirstHundred, new Number1Child(451))); // Control
        Assert.IsTrue(Implementation.Contains(Number1s.FirstHundred, new Number1Child(500), Number1Mod9Comparer));
        Assert.IsTrue(Implementation.Contains(Number1s.FirstHundred, new Number1Child(451), Number1Mod9Comparer));

        // Numbers are not in the collection, but equal something in the collection mod 9
        Assert.IsFalse(Implementation.Contains(Number1s.FirstHundredNoFives, new Number1Child(15))); // Control
        Assert.IsFalse(Implementation.Contains(Number1s.FirstHundredNoFives, new Number1Child(35))); // Control
        Assert.IsTrue(Implementation.Contains(Number1s.FirstHundredNoFives, new Number1Child(40), Number1Mod9Comparer));
        Assert.IsTrue(Implementation.Contains(Number1s.FirstHundredNoFives, new Number1Child(35), Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "Count" methods.
    /// </summary>
    [TestMethod]
    public void TestCount()
    {
        Assert.AreEqual(100, Implementation.Count(Number1s.FirstHundredNoFives));
        Assert.AreEqual(100 - 20, Implementation.Count(Number1s.FirstHundredNoFives, x => x is not null));
        Assert.AreEqual(0, Implementation.Count(Number1s.Empty));
        Assert.AreEqual(0, Implementation.Count(Number1s.Empty, _ => true));
    }
    #endregion

    #region D
    /// <summary>
    /// Tests the "DefaultIfEmpty" methods.
    /// </summary>
    [TestMethod]
    public void TestDefaultIfEmpty()
    {
        Assert.That.AreSequenceEqual(Number1s.FirstHundred, Implementation.DefaultIfEmpty(Number1s.FirstHundred));
        Assert.That.AreSequenceEqual(new Number1?[] { null }, Implementation.DefaultIfEmpty(Number1s.Empty));
    }

    /// <summary>
    /// Tests the "Distinct" methods.
    /// </summary>
    [TestMethod]
    public void TestDistinct()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2),
                                     Implementation.Distinct(NewNumber1s(1, 2, 1, 1, 2)));
        Assert.That.AreSequenceEqual(Number1s.FirstHundred, Implementation.Distinct(Number1s.FirstHundred));
        Assert.That.AreSequenceEqual(Enumerable.Empty<Number1>(), Implementation.Distinct(Number1s.Empty));

        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Take(9),
                                     Implementation.Distinct(Number1s.FirstHundred, Number1Mod9Comparer));
        Assert.That.AreSequenceEqual(Enumerable.Empty<Number1>(),
                                     Implementation.Distinct(Number1s.Empty, Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "DistinctBy" methods.
    /// </summary>
    [TestMethod]
    public void TestDistinctBy()
    {
        Assert.That.AreSequenceEqual(
            Number1s.FirstHundred,
            Implementation.DistinctBy(Number1s.FirstHundred, n1 => new Number2(n1.Value)));

        Assert.That.AreSequenceEqual(
            Number1s.FirstHundred.Where(n => n.Value % 2 == 0), // Every odd will round down
            Implementation.DistinctBy(Number1s.FirstHundred, n1 => new Number2(n1.Value / 2)));

        Assert.That.AreSequenceEqual(Enumerable.Empty<Number1>(),
                                     Implementation.DistinctBy(Number1s.Empty, _ => throw null!));
    }
    #endregion

    #region E
    /// <summary>
    /// Tests the "ElementAt" methods.
    /// </summary>
    [TestMethod]
    public void TestElementAt()
    {
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAt(Number1s.FirstHundred, 2));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Implementation.ElementAt(Number1s.FirstHundred, -1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Implementation.ElementAt(Number1s.FirstHundred, 200));
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAt(Number1s.FirstHundred, new Index(2)));
        Assert.AreEqual(new Number1Child(98), Implementation.ElementAt(Number1s.FirstHundred, ^2));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Implementation.ElementAt(Number1s.FirstHundred, 105));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Implementation.ElementAt(Number1s.FirstHundred, ^105));
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAt(Number1s.FirstHundred, new LongIndex(2)));
        Assert.AreEqual(new Number1Child(98), Implementation.ElementAt(Number1s.FirstHundred, (LongIndex)(^2)));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Implementation.ElementAt(Number1s.FirstHundred, (LongIndex)105));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Implementation.ElementAt(Number1s.FirstHundred, (LongIndex)(^105)));
    }

    /// <summary>
    /// Tests the "ElementAtOrDefault" methods.
    /// </summary>
    [TestMethod]
    public void TestElementAtOrDefault()
    {
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAtOrDefault(Number1s.FirstHundred, 2));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, -1));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, 200));
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAtOrDefault(Number1s.FirstHundred, new Index(2)));
        Assert.AreEqual(new Number1Child(98), Implementation.ElementAtOrDefault(Number1s.FirstHundred, ^2));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, 105));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, ^105));
        Assert.AreEqual(new Number1Child(2), Implementation.ElementAtOrDefault(Number1s.FirstHundred, new LongIndex(2)));
        Assert.AreEqual(new Number1Child(98), Implementation.ElementAtOrDefault(Number1s.FirstHundred, (LongIndex)(^2)));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, (LongIndex)105));
        Assert.IsNull(Implementation.ElementAtOrDefault(Number1s.FirstHundred, (LongIndex)(^105)));
    }

    /// <summary>
    /// Tests the "Except" methods.
    /// </summary>
    [TestMethod]
    public void TestExcept()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8, 9),
                                     Implementation.Except(Number1s.FirstTen,
                                                           NewNumber1Children(0, 0, 4, 6, 7)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8, 9),
                                     Implementation.Except(Number1s.FirstTen,
                                                           EnumerateNumber1s(0, 0, 4, 6, 7)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8, 9),
                                     Implementation.Except(Number1s.FirstTen.AsEnumerable(),
                                                           NewNumber1Children(0, 0, 4, 6, 7)));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8),
                                     Implementation.Except(Number1s.FirstTen,
                                                           NewNumber1Children(0, 0, 4, 6, 7),
                                                           Number1Mod9Comparer));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8),
                                     Implementation.Except(Number1s.FirstTen,
                                                           EnumerateNumber1s(0, 0, 4, 6, 7),
                                                           Number1Mod9Comparer));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 5, 8),
                                     Implementation.Except(Number1s.FirstTen.AsEnumerable(),
                                                           NewNumber1Children(0, 0, 4, 6, 7),
                                                           Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "ExceptBy" methods.
    /// </summary>
    [TestMethod]
    public void TestExceptBy()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 5, 6, 7, 8, 9),
                                     Implementation.ExceptBy(Number1s.FirstTen,
                                                             NewNumber2Children(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(4, 5, 6, 7, 8),
                                     Implementation.ExceptBy(Number1s.FirstTen,
                                                             NewNumber2Children(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2),
                                                             Number2Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 5, 6, 7, 8, 9),
                                     Implementation.ExceptBy(Number1s.FirstTen as IEnumerable<Number1>,
                                                             NewNumber2Children(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(4, 5, 6, 7, 8),
                                     Implementation.ExceptBy(Number1s.FirstTen as IEnumerable<Number1>,
                                                             NewNumber2Children(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2),
                                                             Number2Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 5, 6, 7, 8, 9),
                                     Implementation.ExceptBy(Number1s.FirstTen,
                                                             EnumerateNumber2s(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(4, 5, 6, 7, 8),
                                     Implementation.ExceptBy(Number1s.FirstTen,
                                                             EnumerateNumber2s(2, 6, 4, 9),
                                                             n1 => new Number2Child(n1.Value * 2),
                                                             Number2Mod9Comparer));
    }
    #endregion

    #region F
    /// <summary>
    /// Tests the "First" methods.
    /// </summary>
    [TestMethod]
    public void TestFirst()
    {
        Assert.AreEqual(new Number1Child(0), Implementation.First(Number1s.FirstHundred));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.First(Number1s.Empty));
        Assert.AreEqual(new Number1Child(2), Implementation.First(Number1s.FirstHundred, v => v.Value % 3 == 2));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.First(Number1s.Empty,
                                                                                     _ => throw new Exception()));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.First(NewNumber1s(0, 1, 2),
                                                                                     v => v.Value % 13 > 10));
    }

    /// <summary>
    /// Tests the "FirstOrDefault" methods.
    /// </summary>
    [TestMethod]
    public void TestFirstOrDefault()
    {
        Number1Child defaultValue = 128;

        Assert.AreEqual(new Number1Child(0), Implementation.FirstOrDefault(Number1s.FirstHundred));
        Assert.IsNull(Implementation.FirstOrDefault(Number1s.Empty));
        Assert.AreEqual(new Number1Child(2), Implementation.FirstOrDefault(Number1s.FirstHundred,
                                                                           v => v.Value % 3 == 2));
        Assert.IsNull(Implementation.FirstOrDefault(Number1s.Empty, _ => throw new Exception()));
        Assert.IsNull(Implementation.FirstOrDefault(NewNumber1s(0, 1, 2), v => v.Value % 13 > 10));

        Assert.AreEqual(new Number1Child(0), Implementation.FirstOrDefault(Number1s.FirstHundred, defaultValue));
        Assert.AreEqual(defaultValue, Implementation.FirstOrDefault(Number1s.Empty, defaultValue));
        Assert.AreEqual(new Number1Child(2), Implementation.FirstOrDefault(Number1s.FirstHundred,
                                                                           v => v.Value % 3 == 2, defaultValue));
        Assert.AreEqual(defaultValue, Implementation.FirstOrDefault(Number1s.Empty,
                                                                    _ => throw new Exception(), defaultValue));
        Assert.AreEqual(defaultValue, Implementation.FirstOrDefault(NewNumber1s(0, 1, 2),
                                                                    v => v.Value % 13 > 10, defaultValue));
    }
    #endregion

    #region G
    /// <summary>
    /// Tests the "GroupBy" methods.
    /// </summary>
    [TestMethod]
    public void TestGroupBy()
    {
        Assert.That.AreSequenceEqual(
                        new Grouping<Number2, Number1>[]
                        {
                            new(Key: 0, 0, 1),
                            new(Key: 2, 4, 5),
                            new(Key: 41, 82, 83),
                            new(Key: 50, 100, 101),
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2))
                                      .Select(g => new Grouping<Number2, Number1>(g)),
                        GroupingComparer<Number1>());
        Assert.That.AreSequenceEqual(
                        new Grouping<Number2, Number1>[]
                        {
                            new(Key: 0, 0, 1),
                            new(Key: 2, 4, 5),
                            new(Key: 41, 82, 83, 100, 101),
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               Number2Mod9Comparer)
                                      .Select(g => new Grouping<Number2, Number1>(g)),
                        GroupingComparer<Number1>());

        Assert.That.AreSequenceEqual(
                        new Grouping<Number2, Number3>[]
                        {
                            new(Key: 0, 0 + 1, 1 + 1),
                            new(Key: 2, 4 + 1, 5 + 1),
                            new(Key: 41, 82 + 1, 83 + 1),
                            new(Key: 50, 100 + 1, 101 + 1),
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               x => new Number3(x.Value + 1))
                                      .Select(g => new Grouping<Number2, Number3>(g)),
                        GroupingComparer<Number3>());
        Assert.That.AreSequenceEqual(
                        new Grouping<Number2, Number3>[]
                        {
                            new(Key: 0, 0 + 1, 1 + 1),
                            new(Key: 2, 4 + 1, 5 + 1),
                            new(Key: 41, 82 + 1, 83 + 1, 100 + 1, 101 + 1),
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               x => new Number3(x.Value + 1),
                                               Number2Mod9Comparer)
                                      .Select(g => new Grouping<Number2, Number3>(g)),
                        GroupingComparer<Number3>());

#pragma warning disable IDE0047 // The parens make the solutions easier to trace
        Assert.That.AreSequenceEqual(
                        new BigInteger[]
                        {
                            0 + (0 + 1) + 1,
                            2 + (4 + 5) + 1,
                            41 + (82 + 83) + 1,
                            50 + (100 + 101) + 1,
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               (x, coll) => x.Value + coll.Sum() + 1));
        Assert.That.AreSequenceEqual(
                        new BigInteger[]
                        {
                            0 + (0 + 1) + 1,
                            2 + (4 + 5) + 1,
                            41 + (82 + 83 + 100 + 101) + 1,
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               (x, coll) => x.Value + coll.Sum() + 1,
                                               Number2Mod9Comparer));

        Assert.That.AreSequenceEqual(
                        new BigInteger[]
                        {
                            0 + ((0 + 1) + (1 + 1)) + 1,
                            2 + ((4 + 1) + (5 + 1)) + 1,
                            41 + ((82 + 1) + (83 + 1)) + 1,
                            50 + ((100 + 1) + (101 + 1)) + 1,
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               x => new Number3(x.Value + 1),
                                               (x, coll) => x.Value + coll.Sum() + 1));
        Assert.That.AreSequenceEqual(
                        new BigInteger[]
                        {
                            0 + ((0 + 1) + (1 + 1)) + 1,
                            2 + ((4 + 1) + (5 + 1)) + 1,
                            41 + ((82 + 1) + (83 + 1) + (100 + 1) + (101 + 1)) + 1,
                        },
                        Implementation.GroupBy(NewNumber1s(0, 1, 4, 5, 82, 83, 100, 101),
                                               x => new Number2Child(x.Value / 2),
                                               x => new Number3(x.Value + 1),
                                               (x, coll) => x.Value + coll.Sum() + 1,
                                               Number2Mod9Comparer));
#pragma warning restore IDE0047
    }

    /// <summary>
    /// Tests the "GroupJoin" methods.
    /// </summary>
    [TestMethod]
    public void TestGroupJoin()
    {
        #region Test Setup
        var expectedSequence = new BigInteger[]
        {
            0 + 2 + (8 + 2) * (0 + 1),
            1 + 2 + (8 + 2) * (2 + 3),
            2 + 2, 3 + 2,
            41 + 2 + (8 + 2) * (82 + 83),
            41 + 2 + (8 + 2) * (82 + 83),
            42 + 2,
            50 + 2 + (8 + 2) * (100 + 101),
            50 + 2 + (8 + 2) * (100 + 101),
        };

        var expectedMod9Sequence = new BigInteger[]
        {
            0 + 2 + (8 + 2) * (0 + 1),
            1 + 2 + (8 + 2) * (2 + 3),
            2 + 2, 3 + 2,
            41 + 2 + (8 + 4) * (82 + 83 + 100 + 101),
            41 + 2 + (8 + 4) * (82 + 83 + 100 + 101),
            42 + 2,
            50 + 2 + (8 + 4) * (82 + 83 + 100 + 101),
            50 + 2 + (8 + 4) * (82 + 83 + 100 + 101),
        };
        #endregion

        #region Assertions
        Assert.That.AreSequenceEqual(
                        expectedSequence,
                        Implementation.GroupJoin(NewNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 NewNumber2Children(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum()),
                        showCollections: true);

        Assert.That.AreSequenceEqual(
                        expectedMod9Sequence,
                        Implementation.GroupJoin(NewNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 NewNumber2Children(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum(),
                                                 Number3Mod9Comparer),
                        showCollections: true);

        Assert.That.AreSequenceEqual(
                        expectedSequence,
                        Implementation.GroupJoin(EnumerateNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 NewNumber2Children(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum()),
                        showCollections: true);

        Assert.That.AreSequenceEqual(
                        expectedMod9Sequence,
                        Implementation.GroupJoin(EnumerateNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 NewNumber2Children(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum(),
                                                 Number3Mod9Comparer),
                        showCollections: true);

        Assert.That.AreSequenceEqual(
                        expectedSequence,
                        Implementation.GroupJoin(NewNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 EnumerateNumber2s(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum()),
                        showCollections: true);

        Assert.That.AreSequenceEqual(
                        expectedMod9Sequence,
                        Implementation.GroupJoin(NewNumber1s(0, 1, 2, 3, 41, 41, 42, 50, 50),
                                                 EnumerateNumber2s(0, 1, 2, 3, 82, 83, 100, 101),
                                                 x => new(x.Value + 1),
                                                 x => new(x.Value / 2 + 1),
                                                 (x, coll) => x.Value + 2 + (8 + coll.Count()) * coll.Sum(),
                                                 Number3Mod9Comparer),
                        showCollections: true);
        #endregion
    }
    #endregion

    #region I
    /// <summary>
    /// Tests the "Intersect" methods.
    /// </summary>
    [TestMethod]
    public void TestIntersect()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 2, 4, 6, 7, 8),
                             Implementation.Intersect(Number1s.FirstTen as IEnumerable<Number1>,
                                                      NewNumber1Children(0, 2, 4, 6, 7, 8, 10, 12, 14)));
        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Take(9),
                                     Implementation.Intersect(Number1s.FirstTen as IEnumerable<Number1>,
                                                              NewNumber1Children(0, 2, 4, 6, 7, 8, 10, 12, 14),
                                                              Number1Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 2, 4, 6, 7, 8),
                             Implementation.Intersect(Number1s.FirstTen,
                                                      EnumerateNumber1s(0, 2, 4, 6, 7, 8, 10, 12, 14)));
        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Take(9),
                                     Implementation.Intersect(Number1s.FirstTen,
                                                              EnumerateNumber1s(0, 2, 4, 6, 7, 8, 10, 12, 14),
                                                              Number1Mod9Comparer));


        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 2, 4, 6, 7, 8),
                                     Implementation.Intersect(Number1s.FirstTen,
                                                              NewNumber1Children(0, 2, 4, 6, 7, 8, 10, 12, 14)));
        Assert.That.AreSequenceEqual(Number1s.FirstHundred.Take(9),
                                     Implementation.Intersect(Number1s.FirstTen,
                                                              NewNumber1Children(0, 2, 4, 6, 7, 8, 10, 12, 14),
                                                              Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "IntersectBy" methods.
    /// </summary>
    [TestMethod]
    public void TestIntersectBy()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4),
                                     Implementation.IntersectBy(EnumerateNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                NewNumber2Children(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 24),
                                     Implementation.IntersectBy(EnumerateNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                NewNumber2Children(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2),
                                                                Number2Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4),
                                     Implementation.IntersectBy(NewNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                EnumerateNumber2s(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 24),
                                     Implementation.IntersectBy(NewNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                EnumerateNumber2s(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2),
                                                                Number2Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4),
                                     Implementation.IntersectBy(NewNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                NewNumber2Children(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 4, 24),
                                     Implementation.IntersectBy(NewNumber1s(0, 0, 1, 4, 5, 24, 25),
                                                                NewNumber2Children(0, 0, 1, 2, 3),
                                                                n => new Number2Child(n.Value / 2),
                                                                Number2Mod9Comparer));
    }
    #endregion

    #region J
    /// <summary>
    /// Tests the "Join" methods.
    /// </summary>
    [TestMethod]
    public void TestJoin()
    {
#pragma warning disable IDE0047 // Parens make test results clearer
        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4)
                                     },
                                     Implementation.Join(NewNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         NewNumber2Children(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value));
        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4),
                                         (7 * 14), (12 * 1)
                                     },
                                     Implementation.Join(NewNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         NewNumber2Children(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value,
                                                         Number3Mod9Comparer));

        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4)
                                     },
                                     Implementation.Join(NewNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         EnumerateNumber2s(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value));
        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4),
                                         (7 * 14), (12 * 1)
                                     },
                                     Implementation.Join(NewNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         EnumerateNumber2s(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value,
                                                         Number3Mod9Comparer));

        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4)
                                     },
                                     Implementation.Join(EnumerateNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         NewNumber2Children(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value));
        Assert.That.AreSequenceEqual(new BigInteger[]
                                     {
                                         (3 * 1), (4 * 2), (5 * 3), (5 * 3), (5 * 3), (5 * 3), (6 * 4),
                                         (7 * 14), (12 * 1)
                                     },
                                     Implementation.Join(EnumerateNumber1s(3, 4, 5, 5, 6, 7, 12),
                                                         NewNumber2Children(1, 2, 3, 3, 4, 14),
                                                         k => new(k.Value - 1),
                                                         k => new(k.Value + 1),
                                                         (n1, n2) => n1.Value * n2.Value,
                                                         Number3Mod9Comparer));
#pragma warning restore IDE0047
    }
    #endregion

    #region L
    /// <summary>
    /// Tests the "Last" methods.
    /// </summary>
    [TestMethod]
    public void TestLast()
    {
        Assert.AreEqual(new Number1Child(99), Implementation.Last(Number1s.FirstHundred));
        Assert.AreEqual(new Number1Child(55), Implementation.Last(Number1s.FirstHundred, n => n.Value <= 55));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Last(Number1s.Empty));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Last(Number1s.Empty, _ => throw null!));
    }

    /// <summary>
    /// Tests the "LastOrDefault" methods.
    /// </summary>
    [TestMethod]
    public void TestLastOrDefault()
    {
        Number1 defaultValue = new Number1Child(101);

        Assert.AreEqual(new Number1Child(99), Implementation.LastOrDefault(Number1s.FirstHundred));
        Assert.AreEqual(new Number1Child(55), Implementation.LastOrDefault(Number1s.FirstHundred, n => n.Value <= 55));

        Assert.IsNull(Implementation.LastOrDefault(Number1s.FirstHundred, n => n.Value < 0));
        Assert.IsNull(Implementation.LastOrDefault(Number1s.Empty));
        Assert.IsNull(Implementation.LastOrDefault(Number1s.Empty, _ => throw null!));

        Assert.AreEqual(defaultValue,
                        Implementation.LastOrDefault(Number1s.FirstHundred, n => n.Value < 0, defaultValue));
        Assert.AreEqual(defaultValue, Implementation.LastOrDefault(Number1s.Empty, defaultValue));
        Assert.AreEqual(defaultValue, Implementation.LastOrDefault(Number1s.Empty, _ => throw null!, defaultValue));
    }

    /// <summary>
    /// Tests the "LongCount" methods.
    /// </summary>
    [TestMethod]
    public void TestLongCount()
    {
        Assert.AreEqual(100, Implementation.LongCount(Number1s.FirstHundred));
        Assert.AreEqual(50, Implementation.LongCount(Number1s.FirstHundred, n => n.Value % 2 == 0));
        Assert.AreEqual(0, Implementation.LongCount(Number1s.Empty));
        Assert.AreEqual(0, Implementation.LongCount(Number1s.Empty, _ => throw null!));
    }
    #endregion

    #region M
    /// <summary>
    /// Tests the "Max" methods.
    /// </summary>
    [TestMethod]
    public void TestMax()
    {
        Assert.AreEqual(99, Implementation.Max(Decimals.FirstHundred));
        Assert.AreEqual(99, Implementation.Max(NullableDecimals.FirstHundredNoFives));
        Assert.AreEqual(99, Implementation.Max(Doubles.FirstHundred));
        Assert.AreEqual(99, Implementation.Max(NullableDoubles.FirstHundredNoFives));
        Assert.AreEqual(99, Implementation.Max(Floats.FirstHundred));
        Assert.AreEqual(99, Implementation.Max(NullableFloats.FirstHundredNoFives));
        Assert.AreEqual(99, Implementation.Max(Ints.FirstHundred));
        Assert.AreEqual(99, Implementation.Max(NullableInts.FirstHundredNoFives));
        Assert.AreEqual(99, Implementation.Max(Longs.FirstHundred));
        Assert.AreEqual(99, Implementation.Max(NullableLongs.FirstHundredNoFives));

        Assert.IsNull(Implementation.Max(NullableDecimals.Empty));
        Assert.IsNull(Implementation.Max(NullableDecimals.AllNull));
        Assert.IsNull(Implementation.Max(NullableDoubles.Empty));
        Assert.IsNull(Implementation.Max(NullableDoubles.AllNull));
        Assert.IsNull(Implementation.Max(NullableFloats.Empty));
        Assert.IsNull(Implementation.Max(NullableFloats.AllNull));
        Assert.IsNull(Implementation.Max(NullableInts.Empty));
        Assert.IsNull(Implementation.Max(NullableInts.AllNull));
        Assert.IsNull(Implementation.Max(NullableLongs.Empty));
        Assert.IsNull(Implementation.Max(NullableLongs.AllNull));

        Assert.AreEqual(0, Implementation.Max(Number1s.FirstHundred, d => -(decimal)d.Value));
        Assert.AreEqual(-1, Implementation.Max(Number1s.FirstHundredNoFives, d => -(decimal?)d?.Value));
        Assert.AreEqual(0, Implementation.Max(Number1s.FirstHundred, d => -(double)d.Value));
        Assert.AreEqual(-1, Implementation.Max(Number1s.FirstHundredNoFives, d => -(double?)d?.Value));
        Assert.AreEqual(0, Implementation.Max(Number1s.FirstHundred, d => -(float)d.Value));
        Assert.AreEqual(-1, Implementation.Max(Number1s.FirstHundredNoFives, d => -(float?)d?.Value));
        Assert.AreEqual(0, Implementation.Max(Number1s.FirstHundred, d => -(int)d.Value));
        Assert.AreEqual(-1, Implementation.Max(Number1s.FirstHundredNoFives, d => -(int?)d?.Value));
        Assert.AreEqual(0, Implementation.Max(Number1s.FirstHundred, d => -(long)d.Value));
        Assert.AreEqual(-1, Implementation.Max(Number1s.FirstHundredNoFives, d => -(long?)d?.Value));

        Assert.AreEqual(0, Implementation.Max(Number1s.AllNull, _ => default(decimal)));
        Assert.AreEqual(0, Implementation.Max(Number1s.AllNull, _ => default(double)));
        Assert.AreEqual(0, Implementation.Max(Number1s.AllNull, _ => default(float)));
        Assert.AreEqual(0, Implementation.Max(Number1s.AllNull, _ => default));
        Assert.AreEqual(0, Implementation.Max(Number1s.AllNull, _ => default(long)));

        Assert.AreEqual(new Number1Child(99), Implementation.Max(Number1s.FirstHundred));
        Assert.IsNull(Implementation.Max(Number1s.AllNull));

        Assert.AreEqual(new Number2Child(198),
                        Implementation.Max(Number1s.FirstHundred, n1 => new Number2Child(n1.Value * 2)));
        Assert.AreEqual(new Number2Child(2), Implementation.Max(Number1s.AllNull, _ => new Number2Child(2)));

        Assert.AreEqual(new Number1Child(-8),
                        Implementation.Max(NewNumber1s(-8, -7, 3, 5), Number1AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "MaxBy" methods.
    /// </summary>
    [TestMethod]
    public void TestMaxBy()
    {
        Assert.AreEqual(new Number1Child(3),
                        Implementation.MaxBy(NewNumber1s(-14, -11, -5, 3, 10), x => new Number2Child(x.Value % 5)));

        Assert.AreEqual(new Number1Child(-14),
                        Implementation.MaxBy(NewNumber1s(-14, -11, -5, 3, 10),
                                             x => new Number2Child(x.Value % 5),
                                             Number2AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "Min" methods.
    /// </summary>
    [TestMethod]
    public void TestMin()
    {
        Assert.AreEqual(0, Implementation.Min(Decimals.FirstHundred));
        Assert.AreEqual(1, Implementation.Min(NullableDecimals.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Min(Doubles.FirstHundred));
        Assert.AreEqual(1, Implementation.Min(NullableDoubles.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Min(Floats.FirstHundred));
        Assert.AreEqual(1, Implementation.Min(NullableFloats.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Min(Ints.FirstHundred));
        Assert.AreEqual(1, Implementation.Min(NullableInts.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Min(Longs.FirstHundred));
        Assert.AreEqual(1, Implementation.Min(NullableLongs.FirstHundredNoFives));

        Assert.IsNull(Implementation.Min(NullableDecimals.Empty));
        Assert.IsNull(Implementation.Min(NullableDecimals.AllNull));
        Assert.IsNull(Implementation.Min(NullableDoubles.Empty));
        Assert.IsNull(Implementation.Min(NullableDoubles.AllNull));
        Assert.IsNull(Implementation.Min(NullableFloats.Empty));
        Assert.IsNull(Implementation.Min(NullableFloats.AllNull));
        Assert.IsNull(Implementation.Min(NullableInts.Empty));
        Assert.IsNull(Implementation.Min(NullableInts.AllNull));
        Assert.IsNull(Implementation.Min(NullableLongs.Empty));
        Assert.IsNull(Implementation.Min(NullableLongs.AllNull));

        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundred, d => -(decimal)d.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundredNoFives, d => -(decimal?)d?.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundred, d => -(double)d.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundredNoFives, d => -(double?)d?.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundred, d => -(float)d.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundredNoFives, d => -(float?)d?.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundred, d => -(int)d.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundredNoFives, d => -(int?)d?.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundred, d => -(long)d.Value));
        Assert.AreEqual(-99, Implementation.Min(Number1s.FirstHundredNoFives, d => -(long?)d?.Value));

        Assert.AreEqual(0, Implementation.Min(Number1s.AllNull, _ => default(decimal)));
        Assert.AreEqual(0, Implementation.Min(Number1s.AllNull, _ => default(double)));
        Assert.AreEqual(0, Implementation.Min(Number1s.AllNull, _ => default(float)));
        Assert.AreEqual(0, Implementation.Min(Number1s.AllNull, _ => default));
        Assert.AreEqual(0, Implementation.Min(Number1s.AllNull, _ => default(long)));

        Assert.AreEqual(new Number1Child(0), Implementation.Min(Number1s.FirstHundred));
        Assert.IsNull(Implementation.Min(Number1s.AllNull));

        Assert.AreEqual(new Number2Child(-200),
                        Implementation.Min(Number1s.FirstHundred, n1 => new Number2Child(n1.Value - 100 * 2)));
        Assert.AreEqual(new Number2Child(2), Implementation.Min(Number1s.AllNull, _ => new Number2Child(2)));

        Assert.AreEqual(new Number1Child(3),
                        Implementation.Min(NewNumber1s(-8, -7, 3, 5), Number1AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "MinBy" methods.
    /// </summary>
    [TestMethod]
    public void TestMinBy()
    {
        Assert.AreEqual(new Number1Child(-14),
                        Implementation.MinBy(NewNumber1s(-14, -11, -5, 3, 10), x => new Number2Child(x.Value % 5)));

        Assert.AreEqual(new Number1Child(-5),
                        Implementation.MinBy(NewNumber1s(-14, -11, -5, 3, 10),
                                             x => new Number2Child(x.Value % 5),
                                             Number2AbsoluteValueComparer));
    }
    #endregion

    #region O
    /// <summary>
    /// Tests the "Order" methods.
    /// </summary>
    [TestMethod]
    public void TestOrder()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-4, -1, 2, 3), Implementation.Order(NewNumber1s(-4, 2, 3, -1)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-1, 2, 3, -4),
                                     Implementation.Order(NewNumber1s(-4, 2, 3, -1),
                                                          Number1AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "OrderBy" methods.
    /// </summary>
    [TestMethod]
    public void TestOrderBy()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(3, 2, -1, -4),
                                     Implementation.OrderBy(NewNumber1s(-4, 2, 3, -1),
                                                            n => new Number2Child(-n.Value)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-1, 2, 3, -4),
                                     Implementation.OrderBy(NewNumber1s(-4, 2, 3, -1),
                                                            n => new Number2Child(-n.Value),
                                                            Number2AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "OrderByDescending" methods.
    /// </summary>
    [TestMethod]
    public void TestOrderByDescending()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-4, -1, 2, 3),
                                     Implementation.OrderByDescending(NewNumber1s(-4, 2, 3, -1),
                                                                      n => new Number2Child(-n.Value)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-4, 3, 2, -1),
                                     Implementation.OrderByDescending(NewNumber1s(-4, 2, 3, -1),
                                                                      n => new Number2Child(-n.Value),
                                                                      Number2AbsoluteValueComparer));
    }

    /// <summary>
    /// Tests the "OrderDescending" methods.
    /// </summary>
    [TestMethod]
    public void TestOrderDescending()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(3, 2, -1, -4),
                                     Implementation.OrderDescending(NewNumber1s(-4, 2, 3, -1)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(-4, 3, 2, -1),
                                     Implementation.OrderDescending(NewNumber1s(-4, 2, 3, -1),
                                                                    Number1AbsoluteValueComparer));
    }
    #endregion

    #region P
    /// <summary>
    /// Tests the "Prepend" method.
    /// </summary>
    [TestMethod]
    public void TestPrepend()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4), Implementation.Prepend(NewNumber1s(2, 3, 4), 1));
    }
    #endregion

    #region R
    /// <summary>
    /// Tests the "Reverse" method.
    /// </summary>
    [TestMethod]
    public void TestReverse()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(4, 3, 2, 1), Implementation.Reverse(NewNumber1s(1, 2, 3, 4)));
    }
    #endregion

    #region S
    /// <summary>
    /// Tests the "Select" methods.
    /// </summary>
    [TestMethod]
    public void TestSelect()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber2s(1, 9, 25),
                                     Implementation.Select(NewNumber1s(1, 3, 5),
                                                           n => new Number2Child(n.Value * n.Value)));
        Assert.That.AreSequenceEqual(EnumerateNumber2s(1 + 0, 9 + 1, 25 + 2),
                                     Implementation.Select(NewNumber1s(1, 3, 5),
                                                           (n, i) => new Number2Child(n.Value * n.Value + i)));
    }

    /// <summary>
    /// Tests the "Select" methods.
    /// </summary>
    [TestMethod]
    public void TestSelectMany()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber2s(1, 1, 9, 27, 25, 125),
                                     Implementation.SelectMany(NewNumber1s(1, 3, 5),
                                                               n => new Number2Child[]
                                                                    {
                                                                        new(n.Value * n.Value),
                                                                        new(n.Value * n.Value * n.Value)
                                                                    }));
        Assert.That.AreSequenceEqual(EnumerateNumber2s(1 + 0, 1 + 2, 9 + 1, 27 + 2, 25 + 2, 125 + 2),
                                     Implementation.SelectMany(NewNumber1s(1, 3, 5),
                                                               (n, i) => new Number2Child[]
                                                                         {
                                                                            new(n.Value * n.Value + i),
                                                                            new(n.Value * n.Value * n.Value + 2)
                                                                         }));
    }

    /// <summary>
    /// Tests the "SequenceEqual" methods.
    /// </summary>
    [TestMethod]
    public void TestSequenceEqual()
    {
        Assert.IsTrue(Implementation.SequenceEqual(EnumerateNumber1s(), Number1Children.Empty));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(1), Number1Children.Empty));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(), NewNumber1Children(1)));
        Assert.IsTrue(Implementation.SequenceEqual(EnumerateNumber1s(1, 2, 3), NewNumber1Children(1, 2, 3)));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(1, 2, 3), NewNumber1Children(1, 3, 3)));

        Assert.IsTrue(Implementation.SequenceEqual(EnumerateNumber1s(), Number1Children.Empty, Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(1), Number1Children.Empty, Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(), NewNumber1Children(1), Number1Mod9Comparer));
        Assert.IsTrue(Implementation.SequenceEqual(EnumerateNumber1s(0, 1, 2), NewNumber1Children(9, 10, 11), Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(EnumerateNumber1s(1, 2, 3), NewNumber1Children(1, 3, 3), Number1Mod9Comparer));

        Assert.IsTrue(Implementation.SequenceEqual(Number1s.Empty, EnumerateNumber1s()));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1), EnumerateNumber1s()));
        Assert.IsFalse(Implementation.SequenceEqual(Number1s.Empty, EnumerateNumber1s(1)));
        Assert.IsTrue(Implementation.SequenceEqual(NewNumber1s(1, 2, 3), EnumerateNumber1s(1, 2, 3)));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1, 2, 3), EnumerateNumber1s(1, 3, 3)));

        Assert.IsTrue(Implementation.SequenceEqual(Number1s.Empty, EnumerateNumber1s(), Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1), EnumerateNumber1s(), Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(Number1s.Empty, EnumerateNumber1s(1), Number1Mod9Comparer));
        Assert.IsTrue(Implementation.SequenceEqual(NewNumber1s(0, 1, 2),
                                                   EnumerateNumber1s(9, 10, 11), Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1, 2, 3),
                                                    EnumerateNumber1s(1, 3, 3), Number1Mod9Comparer));

        Assert.IsTrue(Implementation.SequenceEqual(Number1s.Empty, Number1Children.Empty));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1), Number1Children.Empty));
        Assert.IsFalse(Implementation.SequenceEqual(Number1s.Empty, NewNumber1Children(1)));
        Assert.IsTrue(Implementation.SequenceEqual(NewNumber1s(1, 2, 3), NewNumber1Children(1, 2, 3)));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1, 2, 3), NewNumber1Children(1, 3, 3)));

        Assert.IsTrue(Implementation.SequenceEqual(Number1s.Empty, Number1Children.Empty, Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1), Number1Children.Empty, Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(Number1s.Empty, NewNumber1Children(1), Number1Mod9Comparer));
        Assert.IsTrue(Implementation.SequenceEqual(NewNumber1s(0, 1, 2),
                                                   NewNumber1Children(9, 10, 11), Number1Mod9Comparer));
        Assert.IsFalse(Implementation.SequenceEqual(NewNumber1s(1, 2, 3),
                                                    NewNumber1Children(1, 3, 3), Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "Single" methods.
    /// </summary>
    [TestMethod]
    public void TestSingle()
    {
        Assert.AreEqual(new Number1Child(4), Implementation.Single(NewNumber1s(4)));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Single(Number1s.Empty));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Single(NewNumber1s(4, 5)));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Single(NewNumber1s(4),
                                                                                      x => x.Value % 2 != 0));
        Assert.AreEqual(new Number1Child(5), Implementation.Single(NewNumber1s(4, 5), x => x.Value % 2 != 0));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.Single(NewNumber1s(4, 5, 7),
                                                                                      x => x.Value % 2 != 0));
    }

    /// <summary>
    /// Tests the "SingleOrDefault" methods.
    /// </summary>
    [TestMethod]
    public void TestSingleOrDefault()
    {
        var defaultValue = new Number1Child(16);

        Assert.AreEqual(new Number1Child(4), Implementation.SingleOrDefault(NewNumber1s(4)));
        Assert.IsNull(Implementation.SingleOrDefault(Number1s.Empty));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.SingleOrDefault(NewNumber1s(4, 5)));
        Assert.IsNull(Implementation.SingleOrDefault(NewNumber1s(4), x => x.Value % 2 != 0));
        Assert.AreEqual(new Number1Child(5), Implementation.SingleOrDefault(NewNumber1s(4, 5), x => x.Value % 2 != 0));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.SingleOrDefault(NewNumber1s(4, 5, 7),
                                                                                               x => x.Value % 2 != 0));

        Assert.AreEqual(new Number1Child(4), Implementation.SingleOrDefault(NewNumber1s(4), defaultValue));
        Assert.AreEqual(defaultValue, Implementation.SingleOrDefault(Number1s.Empty, defaultValue));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.SingleOrDefault(NewNumber1s(4, 5),
                                                                                               defaultValue));
        Assert.AreEqual(defaultValue, Implementation.SingleOrDefault(NewNumber1s(4),
                                                                     x => x.Value % 2 != 0, defaultValue));
        Assert.AreEqual(new Number1Child(5), Implementation.SingleOrDefault(NewNumber1s(4, 5),
                                                                            x => x.Value % 2 != 0,
                                                                            defaultValue));
        Assert.ThrowsException<InvalidOperationException>(() => Implementation.SingleOrDefault(NewNumber1s(4, 5, 7),
                                                                     x => x.Value % 2 != 0,
                                                                     defaultValue));
    }

    /// <summary>
    /// Tests the "Skip" methods.
    /// </summary>
    [TestMethod]
    public void TestSkip()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.Skip(NewNumber1s(0, 1, 2, 3), 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3), Implementation.Skip(NewNumber1s(0, 1, 2, 3), 1));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.Skip(NewNumber1s(0, 1, 2, 3), 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.Skip(NewNumber1s(0, 1, 2, 3), 5));
    }

    /// <summary>
    /// Tests the "SkipLast" methods.
    /// </summary>
    [TestMethod]
    public void TestSkipLast()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.SkipLast(NewNumber1s(0, 1, 2, 3), 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2), Implementation.SkipLast(NewNumber1s(0, 1, 2, 3), 1));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.SkipLast(NewNumber1s(0, 1, 2, 3), 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.SkipLast(NewNumber1s(0, 1, 2, 3), 5));
    }

    /// <summary>
    /// Tests the "SkipWhile" methods.
    /// </summary>
    [TestMethod]
    public void TestSkipWhile()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                             x => x.Value < 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(2, 3), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                       x => x.Value < 2));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                   x => x.Value < 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                   x => x.Value < 5));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3), (x, i) => x.Value - i < 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                          (x, i) => x.Value + i < 2));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(3), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                    (x, i) => x.Value + i < 6));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.SkipWhile(NewNumber1s(0, 1, 2, 3),
                                                                                   (x, i) => x.Value + i < 7));
    }

    /// <summary>
    /// Tests the "Sum" methods.
    /// </summary>
    [TestMethod]
    public void TestSum()
    {
        Assert.AreEqual(0, Implementation.Sum(Decimals.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableDecimals.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableDecimals.AllNull));
        Assert.AreEqual(TestCollections.FirstHundredSum, Implementation.Sum(Decimals.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesSum,
                        Implementation.Sum(NullableDecimals.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Sum(Number1s.Empty, x => (decimal)-x.Value));
        Assert.AreEqual(-TestCollections.FirstHundredSum,
                        Implementation.Sum(Number1s.FirstHundred, x => (decimal)-x.Value));

        Assert.AreEqual(0, Implementation.Sum(Doubles.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableDoubles.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableDoubles.AllNull));
        Assert.AreEqual(TestCollections.FirstHundredSum, Implementation.Sum(Doubles.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesSum,
                        Implementation.Sum(NullableDoubles.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Sum(Number1s.Empty, x => (double)-x.Value));
        Assert.AreEqual(-TestCollections.FirstHundredSum,
                        Implementation.Sum(Number1s.FirstHundred, x => (double)-x.Value));

        Assert.AreEqual(0, Implementation.Sum(Floats.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableFloats.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableFloats.AllNull));
        Assert.AreEqual(TestCollections.FirstHundredSum, Implementation.Sum(Floats.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesSum,
                        Implementation.Sum(NullableFloats.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Sum(Number1s.Empty, x => (float)-x.Value));
        Assert.AreEqual(-TestCollections.FirstHundredSum,
                        Implementation.Sum(Number1s.FirstHundred, x => (float)-x.Value));

        Assert.AreEqual(0, Implementation.Sum(Ints.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableInts.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableInts.AllNull));
        Assert.AreEqual(TestCollections.FirstHundredSum, Implementation.Sum(Ints.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesSum,
                        Implementation.Sum(NullableInts.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Sum(Number1s.Empty, x => (int)-x.Value));
        Assert.AreEqual(-TestCollections.FirstHundredSum,
                        Implementation.Sum(Number1s.FirstHundred, x => (int)-x.Value));

        Assert.AreEqual(0, Implementation.Sum(Longs.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableLongs.Empty));
        Assert.AreEqual(0, Implementation.Sum(NullableLongs.AllNull));
        Assert.AreEqual(TestCollections.FirstHundredSum, Implementation.Sum(Longs.FirstHundred));
        Assert.AreEqual(TestCollections.FirstHundredNoFivesSum,
                        Implementation.Sum(NullableLongs.FirstHundredNoFives));
        Assert.AreEqual(0, Implementation.Sum(Number1s.Empty, x => (long)-x.Value));
        Assert.AreEqual(-TestCollections.FirstHundredSum,
                        Implementation.Sum(Number1s.FirstHundred, x => (long)-x.Value));
    }
    #endregion

    #region T
    /// <summary>
    /// Tests the "Take" methods.
    /// </summary>
    [TestMethod]
    public void TestTake()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.Take(NewNumber1s(0, 1, 2, 3), 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0), Implementation.Take(NewNumber1s(0, 1, 2, 3), 1));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.Take(NewNumber1s(0, 1, 2, 3), 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.Take(NewNumber1s(0, 1, 2, 3), 5));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.Take(NewNumber1s(0, 1, 2, 3), 2..2));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2), Implementation.Take(NewNumber1s(0, 1, 2, 3), 1..3));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3), Implementation.Take(NewNumber1s(0, 1, 2, 3), 1..4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.Take(NewNumber1s(0, 1, 2, 3), 0..4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2),
                                     Implementation.Take(NewNumber1s(0, 1, 2, 3), ^5..3));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3),
                                     Implementation.Take(NewNumber1s(0, 1, 2, 3), 1..6));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.Take(NewNumber1s(0, 1, 2, 3), ^5..6));
    }

    /// <summary>
    /// Tests the "TakeLast" methods.
    /// </summary>
    [TestMethod]
    public void TestTakeLast()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(),
                                     Implementation.TakeLast(NewNumber1s(0, 1, 2, 3), 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(3), Implementation.TakeLast(NewNumber1s(0, 1, 2, 3), 1));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.TakeLast(NewNumber1s(0, 1, 2, 3), 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.TakeLast(NewNumber1s(0, 1, 2, 3), 5));
    }

    /// <summary>
    /// Tests the "TakeWhile" methods.
    /// </summary>
    [TestMethod]
    public void TestTakeWhile()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(), Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3),
                                                                                   x => x.Value < 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1), Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3),
                                                                                       x => x.Value < 2));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3), x => x.Value < 4));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3), x => x.Value < 5));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(),
                                     Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3), (x, i) => x.Value - i < 0));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0), Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3),
                                                                                    (x, i) => x.Value + i < 2));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2),
                                     Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3), (x, i) => x.Value + i < 6));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.TakeWhile(NewNumber1s(0, 1, 2, 3), (x, i) => x.Value + i < 7));
    }

    /// <summary>
    /// Tests the "ToArray" method.
    /// </summary>
    [TestMethod]
    public void TestToArray()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.ToArray(NewNumber1s(0, 1, 2, 3)));
    }

    /// <summary>
    /// Tests the "ToImmutableArray" method.
    /// </summary>
    [TestMethod]
    public void TestToImmutableArray()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.ToImmutableArray(NewNumber1s(0, 1, 2, 3)));
    }

    /// <summary>
    /// Tests the "ToImmutableHashSet" methods.
    /// </summary>
    [TestMethod]
    public void TestToImmutableHashSet()
    {
        Assert.IsTrue(EnumerateNumber1s(0, 1, 9, 10)
                        .ToHashSet()
                        .SetEquals(Implementation.ToImmutableHashSet(NewNumber1s(0, 1, 9, 10))));

        Assert.IsTrue(EnumerateNumber1s(0, 1)
                        .ToHashSet()
                        .SetEquals(Implementation.ToImmutableHashSet(NewNumber1s(0, 1, 9, 10), Number1Mod9Comparer)));
    }

    /// <summary>
    /// Tests the "ToImmutableList" method.
    /// </summary>
    [TestMethod]
    public void TestToImmutableList()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3),
                                     Implementation.ToImmutableList(NewNumber1s(0, 1, 2, 3)));
    }

    /// <summary>
    /// Tests the "ToList" method.
    /// </summary>
    [TestMethod]
    public void TestToList()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 2, 3), Implementation.ToList(NewNumber1s(0, 1, 2, 3)));
    }

    /// <summary>
    /// Tests the "ToLookup" methods.
    /// </summary>
    [TestMethod]
    public void TestToLookup()
    {
        Assert.That.AreSequenceEqual(new Grouping<Number2Child, Number1Child>[]
                                     {
                                         new(Key: 0, 0, 0, 1), new(Key: 1, 5), new(Key: 2, 7, 8),
                                         new(Key: 3, 9), new(Key: 5, 16), new(Key: 14, 43),
                                     } as IEnumerable<IGrouping<Number2, Number1>>,
                                     Implementation.ToLookup(NewNumber1s(0, 0, 1, 5, 7, 8, 9, 16, 43),
                                                             n => new Number2Child(n.Value / 3))
                                                   .Select(g => new Grouping<Number2, Number1>(g)),
                                     GroupingComparer<Number1>());
        Assert.That.AreSequenceEqual(new Grouping<Number2Child, Number1Child>[]
                                     {
                                         new(Key: 0, 0, 0, 1), new(Key: 1, 5), new(Key: 2, 7, 8),
                                         new(Key: 3, 9), new(Key: 5, 16, 43),
                                     } as IEnumerable<IGrouping<Number2, Number1>>,
                                     Implementation.ToLookup(NewNumber1s(0, 0, 1, 5, 7, 8, 9, 16, 43),
                                                             n => new Number2Child(n.Value / 3),
                                                             Number2Mod9Comparer)
                                                   .Select(g => new Grouping<Number2, Number1>(g)),
                                     GroupingComparer<Number1>());

        Assert.That.AreSequenceEqual(new Grouping<Number2Child, Number3>[]
                                     {
                                         new(Key: 0, 1, 1, 2), new(Key: 1, 6), new(Key: 2, 8, 9),
                                         new(Key: 3, 10), new(Key: 5, 17), new(Key: 14, 44),
                                     } as IEnumerable<IGrouping<Number2, Number3>>,
                                     Implementation.ToLookup(NewNumber1s(0, 0, 1, 5, 7, 8, 9, 16, 43),
                                                             n => new Number2Child(n.Value / 3),
                                                             n => new Number3(n.Value + 1))
                                                   .Select(g => new Grouping<Number2, Number3>(g)),
                                     GroupingComparer<Number3>());
        Assert.That.AreSequenceEqual(new Grouping<Number2Child, Number3>[]
                                     {
                                         new(Key: 0, 1, 1, 2), new(Key: 1, 6), new(Key: 2, 8, 9),
                                         new(Key: 3, 10), new(Key: 5, 17, 44),
                                     } as IEnumerable<IGrouping<Number2, Number3>>,
                                     Implementation.ToLookup(NewNumber1s(0, 0, 1, 5, 7, 8, 9, 16, 43),
                                                             n => new Number2Child(n.Value / 3),
                                                             n => new Number3(n.Value + 1),
                                                             Number2Mod9Comparer)
                                                   .Select(g => new Grouping<Number2, Number3>(g)),
                                     GroupingComparer<Number3>());
    }

    /// <summary>
    /// Tests the "TryGetNonEnumeratedCount" method.
    /// </summary>
    [TestMethod]
    public void TestTryGetNonEnumeratedCount()
    {
        Assert.IsTrue(Implementation.TryGetNonEnumeratedCount(Number1s.Empty, out var count));
        Assert.AreEqual(0, count);
        Assert.IsTrue(Implementation.TryGetNonEnumeratedCount(Number1s.FirstHundred, out count));
        Assert.AreEqual(100, count);
    }
    #endregion

    #region U
    /// <summary>
    /// Tests the "Union" methods.
    /// </summary>
    [TestMethod]
    public void TestUnion()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 0, 5, 9),
                                     Implementation.Union(NewNumber1s(1, 1, 2, 3, 4, 18),
                                                          NewNumber1Children(0, 3, 5, 9)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 5),
                                     Implementation.Union(NewNumber1s(1, 1, 2, 3, 4, 18),
                                                          NewNumber1Children(0, 3, 5, 9),
                                                          Number1Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 0, 5, 9),
                                     Implementation.Union(EnumerateNumber1s(1, 1, 2, 3, 4, 18),
                                                          NewNumber1Children(0, 3, 5, 9)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 5),
                                     Implementation.Union(EnumerateNumber1s(1, 1, 2, 3, 4, 18),
                                                          NewNumber1Children(0, 3, 5, 9),
                                                          Number1Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 0, 5, 9),
                                     Implementation.Union(NewNumber1s(1, 1, 2, 3, 4, 18),
                                                          EnumerateNumber1s(0, 3, 5, 9)));
        Assert.That.AreSequenceEqual(EnumerateNumber1s(1, 2, 3, 4, 18, 5),
                                     Implementation.Union(NewNumber1s(1, 1, 2, 3, 4, 18),
                                                          EnumerateNumber1s(0, 3, 5, 9),
                                                          Number1Mod9Comparer));
    }

    /// <summary>
    /// Tests the "UnionBy" methods.
    /// </summary>
    [TestMethod]
    public void TestUnionBy()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 9, 10, 11, 2, 3, 18),
                                     Implementation.UnionBy(EnumerateNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            NewNumber1Children(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22)));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 11, 3),
                                     Implementation.UnionBy(EnumerateNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            NewNumber1Children(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22),
                                                            Number3Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 9, 10, 11, 2, 3, 18),
                                     Implementation.UnionBy(NewNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            EnumerateNumber1s(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22)));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 11, 3),
                                     Implementation.UnionBy(NewNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            EnumerateNumber1s(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22),
                                                            Number3Mod9Comparer));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 9, 10, 11, 2, 3, 18),
                                     Implementation.UnionBy(NewNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            NewNumber1Children(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22)));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 5, 11, 3),
                                     Implementation.UnionBy(NewNumber1s(0, 0, 1, 5, 9, 10, 11, 23),
                                                            NewNumber1Children(1, 2, 3, 18, 49),
                                                            n => new Number3(n.Value % 22),
                                                            Number3Mod9Comparer));
    }
    #endregion

    #region W
    /// <summary>
    /// Tests the "Where" methods.
    /// </summary>
    [TestMethod]
    public void TestWhere()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 2, 4),
                                     Implementation.Where(NewNumber1s(0, 1, 2, 3, 4), x => x.Value % 2 == 0));

        Assert.That.AreSequenceEqual(EnumerateNumber1s(0, 1, 10),
                                     Implementation.Where(NewNumber1s(0, 1, 3, 6, 10), (x, i) => x.Value % 2 == i % 2));
    }
    #endregion

    #region Z
    /// <summary>
    /// Tests the "Zip" methods.
    /// </summary>
    [TestMethod]
    public void TestZip()
    {
        Assert.That.AreSequenceEqual(EnumerateNumber3s(2, 6, 12),
                                     Implementation.Zip(EnumerateNumber1s(1, 2, 3),
                                                        NewNumber2Children(2, 3, 4, 5),
                                                        (n1, n2) => new Number3(n1.Value * n2.Value)));
        Assert.That.AreSequenceEqual(EnumerateNumber3s(2, 6, 12),
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        EnumerateNumber2s(2, 3, 4, 5),
                                                        (n1, n2) => new Number3(n1.Value * n2.Value)));

        Assert.That.AreSequenceEqual(EnumerateNumber3s(2, 6, 12),
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        NewNumber2Children(2, 3, 4, 5),
                                                        (n1, n2) => new Number3(n1.Value * n2.Value)));

        var expectedPairs = new (Number1Child, Number2Child)[] { (1, 2), (2, 3), (3, 4) }
                                    .Select(p => (p.Item1 as Number1, p.Item2 as Number2));
        Assert.That.AreSequenceEqual(expectedPairs,
                                     Implementation.Zip(EnumerateNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5)));
        Assert.That.AreSequenceEqual(expectedPairs,
                                 Implementation.Zip(NewNumber1s(1, 2, 3),
                                                    EnumerateNumber2s(2, 3, 4, 5)));
        Assert.That.AreSequenceEqual(expectedPairs,
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5)));

        var expectedTriples = new (Number1Child, Number2Child, Number3)[] { (1, 2, 10), (2, 3, 100) }
                                    .Select(p => (p.Item1 as Number1, p.Item2 as Number2, p.Item3));
        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(EnumerateNumber1s(1, 2, 3),
                                                        EnumerateNumber2s(2, 3, 4, 5),
                                                        NewNumber3s(10, 100)));
        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        EnumerateNumber2s(2, 3, 4, 5),
                                                        EnumerateNumber3s(10, 100)));
        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(EnumerateNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5),
                                                        EnumerateNumber3s(10, 100)));

        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(EnumerateNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5),
                                                        NewNumber3s(10, 100)));
        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        EnumerateNumber2s(2, 3, 4, 5),
                                                        NewNumber3s(10, 100)));
        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5),
                                                        EnumerateNumber3s(10, 100)));

        Assert.That.AreSequenceEqual(expectedTriples,
                                     Implementation.Zip(NewNumber1s(1, 2, 3),
                                                        NewNumber2s(2, 3, 4, 5),
                                                        NewNumber3s(10, 100)));
    }
    #endregion
    #endregion

    #region Constructors
    protected TNumber1Collection NewNumber1s(params Number1Child[] elements)
        => NewNumber1s(elements as IEnumerable<Number1Child>);
    protected TNumber1ACollection NewNumber1Children(params Number1Child[] elements)
        => NewNumber1Children(elements as IEnumerable<Number1Child>);

    protected TNumber2Collection NewNumber2s(params Number2Child[] elements)
        => NewNumber2s(elements as IEnumerable<Number2Child>);
    protected TNumber2ACollection NewNumber2Children(params Number2Child[] elements)
        => NewNumber2Children(elements as IEnumerable<Number2Child>);

    protected TNumber3Collection NewNumber3s(params Number3[] elements)
        => NewNumber3s(elements as IEnumerable<Number3>);

    protected static IEnumerable<Number1> EnumerateNumber1s(params Number1Child[] elements) => elements;
    protected static IEnumerable<Number2> EnumerateNumber2s(params Number2Child[] elements) => elements;
    protected static IEnumerable<Number3> EnumerateNumber3s(params Number3[] elements) => elements;
    #endregion
    #endregion

    #region Abstract
    #region Properties
    /// <summary>
    /// Gets the implementation being tested.
    /// </summary>
    protected abstract ILinqImplementation<TNumber1Collection, Number1,
                                           TNumber1Chunk,
                                           TNumber1ACollection, Number1Child,
                                           TNumber2Collection, Number2,
                                           TNumber2ACollection, Number2Child,
                                           TNumber3Collection, Number3,
                                           BigInteger,
                                           TDecimalCollection, TNullableDecimalCollection,
                                           TDoubleCollection, TNullableDoubleCollection,
                                           TFloatCollection, TNullableFloatCollection,
                                           TIntCollection, TNullableIntCollection,
                                           TLongCollection, TNullableLongCollection> Implementation
    { get; }
    #endregion

    #region Constructors
    protected abstract TNumber1Collection NewNumber1s(IEnumerable<Number1Child> elements);
    protected abstract TNumber1Chunk NewChunk(IEnumerable<Number1Child> elements);
    protected abstract TNumber1ACollection NewNumber1Children(IEnumerable<Number1Child> elements);
    protected abstract TNumber2Collection NewNumber2s(IEnumerable<Number2Child> elements);
    protected abstract TNumber2ACollection NewNumber2Children(IEnumerable<Number2Child> elements);
    protected abstract TNumber3Collection NewNumber3s(IEnumerable<Number3> elements);
    protected abstract TDecimalCollection New(IEnumerable<decimal> elements);
    protected abstract TDoubleCollection New(IEnumerable<double> elements);
    protected abstract TFloatCollection New(IEnumerable<float> elements);
    protected abstract TIntCollection New(IEnumerable<int> elements);
    protected abstract TLongCollection New(IEnumerable<long> elements);
    protected abstract TNullableDecimalCollection NewNullable(IEnumerable<decimal?> elements);
    protected abstract TNullableDoubleCollection NewNullable(IEnumerable<double?> elements);
    protected abstract TNullableFloatCollection NewNullable(IEnumerable<float?> elements);
    protected abstract TNullableIntCollection NewNullable(IEnumerable<int?> elements);
    protected abstract TNullableLongCollection NewNullable(IEnumerable<long?> elements);
    #endregion
    #endregion
}

public sealed record class Grouping<TKey, TValue>(TKey Key, IEnumerable<TValue> Values) : IGrouping<TKey, TValue>
where TKey : notnull
{
    public IEnumerable<TValue> Values
    {
        get => _values;
        init => _values = value ?? throw new ArgumentNullException(nameof(Values));
    }
    private readonly IEnumerable<TValue> _values = Values ?? throw new ArgumentNullException(nameof(Values));

    public Grouping(TKey Key, params TValue[] Values) : this(Key, Values as IEnumerable<TValue>) { }

    public Grouping(IGrouping<TKey, TValue> other) : this(other.Key, other) { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Values.GetEnumerator();

    public bool Equals(Grouping<TKey, TValue>? other) => other is not null
                                                            && EqualityComparer<TKey>.Default.Equals(Key, other.Key)
                                                            && Values.SequenceEqual(other.Values);
    public override int GetHashCode() => HashCode.Combine(Key, Values.GetSequenceHashCode());

    public override string ToString()
        => $"Grouping<{typeof(TKey)}, {typeof(TValue)}> {{ Key = {Key}, Values = {{ {string.Join(", ", Values)} }} }}";
}

#region Comparers
/// <summary>
/// Compares two instances based on their absolute values.
/// </summary>
/// <typeparam name="TNumber"></typeparam>
public sealed class AbsoluteValueComparer<TNumber> : ClassComparer<TNumber> where TNumber : NumberBase
{
    protected override int CompareNonNull(TNumber x, TNumber y)
    {
        return BigInteger.Abs(x.Value).CompareTo(BigInteger.Abs(y.Value));
    }
}

/// <summary>
/// Compares two nullable members of a class, treating two <see langword="null"/> values as equal and treating
/// <see langword="null"/> as the lesser of <see langword="null"/> and a non-<see langword="null"/> value.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ClassComparer<T> : IComparer<T> where T : class
{
    public int Compare(T? x, T? y)
    {
        if (x is null) return y is null ? 0 : -1;
        else if (y is null) return 1;
        else return CompareNonNull(x, y);
    }
    protected abstract int CompareNonNull(T x, T y);
}
#endregion

#region EqualityComparers
public sealed class GroupingEqualityComparer<TKey, TValue> : ClassEqualityComparer<IGrouping<TKey, TValue>>
where TKey : notnull
{
    protected override bool EqualsNotNull(IGrouping<TKey, TValue> x, IGrouping<TKey, TValue> y)
    {
        return EqualityComparer<TKey>.Default.Equals(x.Key, y.Key) && x.SequenceEqual(y);
    }

    public override int GetHashCode(IGrouping<TKey, TValue> obj)
    {
        return EqualityComparer<TKey>.Default.GetHashCode(obj.Key);
    }
}

public sealed class EqualMod9BucketMod3Comparer<TNumber> : ClassEqualityComparer<TNumber> where TNumber : NumberBase
{
    public override int GetHashCode(TNumber obj)
    {
        return obj.Value.Mod(3).GetHashCode();
    }

    protected override bool EqualsNotNull(TNumber x, TNumber y)
    {
        return x.Value.Mod(9) == y.Value.Mod(9);
    }
}

public abstract class ClassEqualityComparer<T> : EqualityComparer<T> where T : class
{
    public sealed override bool Equals(T? x, T? y) => x is null
                                                        ? y is null
                                                        : y is not null
                                                            && EqualsNotNull(x, y);
    protected abstract bool EqualsNotNull(T x, T y);
}
#endregion

#region Test Collections
public sealed class ReferenceTestCollections<TCollection, TElement> : TestCollections<TCollection, TElement>
where TCollection : IEnumerable<TElement>
where TElement : class
{
    public TCollection FirstHundredNoFives { get; }
    public TCollection AllNull { get; }
    public ReferenceTestCollections(TCollection FirstTen, TCollection FirstHundred, TCollection Empty,
                                    TCollection FirstHundredNoFives, TCollection AllNull)
        : base(FirstTen, FirstHundred, Empty)
    {
        this.FirstHundredNoFives = FirstHundredNoFives;
        this.AllNull = AllNull;
    }
}

public class NullableTestCollections<TCollection, TElement> : TestCollectionsBase<TCollection, TElement?>
where TCollection : IEnumerable<TElement?>
where TElement : struct
{
    public TCollection FirstHundredNoFives { get; }
    public TCollection AllNull { get; }
    public TCollection FirstTen { get; }
    public NullableTestCollections(TCollection FirstTen, TCollection FirstHundredNoFives, TCollection Empty, TCollection AllNull)
        : base(Empty)
    {
        this.FirstTen = FirstTen;
        this.FirstHundredNoFives = FirstHundredNoFives;
        this.AllNull = AllNull;
    }
}

public class TestCollections<TCollection, TElement> : TestCollectionsBase<TCollection, TElement>
where TCollection : IEnumerable<TElement>
{
    public TCollection FirstTen { get; }
    public TCollection FirstHundred { get; }
    public TestCollections(TCollection FirstTen, TCollection FirstHundred, TCollection Empty) : base(Empty)
    {
        this.FirstTen = FirstTen;
        this.FirstHundred = FirstHundred;
    }
}

public abstract class TestCollectionsBase<TCollection, TElement> where TCollection : IEnumerable<TElement>
{
    [NotNull] public TCollection Empty { get; }
    protected TestCollectionsBase(TCollection Empty) { this.Empty = Empty; }
}

public static class TestCollections
{
    public static readonly IEnumerable<int> FirstTenRange = Enumerable.Range(0, 10);

    public static readonly IEnumerable<int> FirstHundredRange = Enumerable.Range(0, 100);
    private static readonly IEnumerable<float> FirstHundredFloatRange = FirstHundredRange.Select(i => (float)i);

    public static readonly int FirstHundredSum = FirstHundredRange.Sum();

    public static readonly decimal FirstHundredDecimalAverage = FirstHundredRange.Select(i => (decimal)i).Average();
    public static readonly double FirstHundredDoubleAverage = FirstHundredRange.Average();
    public static readonly float FirstHundredFloatAverage = FirstHundredFloatRange.Average();

    public static readonly IEnumerable<int?> FirstHundredNoFivesRange
        = FirstHundredRange.Select(n => n % 5 == 0 ? default(int?) : n);
    public static readonly IEnumerable<float?> FirstHundredNoFivesFloatRange
        = FirstHundredNoFivesRange.Select(i => (float?)i);

    public static readonly int FirstHundredNoFivesSum = (int)FirstHundredNoFivesRange.Sum()!;

    public static readonly decimal FirstHundredNoFivesDecimalAverage
        = (decimal)FirstHundredNoFivesRange.Select(i => (decimal?)i).Average()!;
    public static readonly double FirstHundredNoFivesDoubleAverage = (double)FirstHundredNoFivesRange.Average()!;
    public static readonly float FirstHundredNoFivesFloatAverage = (float)FirstHundredNoFivesFloatRange.Average()!;
}
#endregion

#region Test Classes
public record class Number1(BigInteger Value) : NumberBase(Value), IComparable<Number1>
{
    [return: NotNullIfNotNull(nameof(first)), NotNullIfNotNull(nameof(second))]
    public static Number1? Add(Number1? first, Number1? second)
    {
        if (first is null) return second;
        else if (second is null) return first;
        else return new(first.Value + second.Value);
    }

    [return: NotNullIfNotNull(nameof(first)), NotNullIfNotNull(nameof(second))]
    public static Number2? Add(Number2? first, Number1? second)
    {
        if (first is null) return second is null ? null : new(second.Value);
        else if (second is null) return first;
        else return new(first.Value + second.Value);
    }

    public int CompareTo(Number1? other)
    {
        return other is null ? -1 : Value.CompareTo(other.Value);
    }

    public static implicit operator Number1(int n) => new Number1Child(Value: n);
}
public sealed record class Number1Child(BigInteger Value) : Number1(Value)
{
    public static implicit operator Number1Child(int n) => new(Value: n);
}

public record class Number2(BigInteger Value) : NumberBase(Value), IComparable<Number2>
{
    public static implicit operator Number2(int n) => new Number2Child(Value: n);

    public int CompareTo(Number2? other)
    {
        return other is null ? -1 : Value.CompareTo(other.Value);
    }
}
public sealed record class Number2Child(BigInteger Value) : Number2(Value)
{
    public static implicit operator Number2Child(int n) => new(Value: n);
}

public sealed record class Number3(BigInteger Value) : NumberBase(Value)
{
    public static implicit operator Number3(int n) => new(Value: n);
}

public abstract record class NumberBase(BigInteger Value);
#endregion

#region Helpers
file static class BigIntegers
{
    /// <summary>
    /// Gets the modulus of the current <see cref="BigInteger"/> and a <see cref="BigInteger"/> divisor.
    /// </summary>
    /// <param name="dividend">The current <see cref="BigInteger"/>.</param>
    /// <param name="divisor">A <see cref="BigInteger"/> divisor to use to compute the modulus.</param>
    /// <returns>
    /// <paramref name="dividend"/> (mod <paramref name="divisor"/>).
    /// This value will never be negative.
    /// </returns>
    public static BigInteger Mod(this BigInteger dividend, BigInteger divisor) => BigInteger.Abs(dividend) % divisor;

    public static BigInteger Sum<TNumber>(this IEnumerable<TNumber> source) where TNumber : NumberBase
        => source.Aggregate(BigInteger.Zero, (n1, n2) => n1 + n2.Value);

    public static BigInteger Product(this IEnumerable<BigInteger> source)
    {
        var product = BigInteger.One;
        foreach (var item in source)
        {
            if (item.IsZero) return BigInteger.Zero;
            else product *= item;
        }
        return product;
    }
}
#endregion
