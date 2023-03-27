using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

/// <summary>
/// Helper methods for running tests on LINQ extension methods.
/// </summary>
/// <typeparam name="TFloatCollection"></typeparam>
/// <typeparam name="TStringCollection"></typeparam>
internal abstract class LinqExtensionsTestHelpers<TFloatCollection, TStringCollection>
    where TFloatCollection : IEnumerable<float>
    where TStringCollection : IEnumerable<string>
{
    /// <summary>
    /// Gets an object that contains helper logic for working with <typeparamref name="TFloatCollection"/>.
    /// </summary>
    protected TestHelpers<TFloatCollection, float> FloatTestMethods { get; }

    /// <summary>
    /// Gets an object that contains helper logic for working with <typeparamref name="TStringCollection"/>.
    /// </summary>
    protected TestHelpers<TStringCollection, string> StringTestMethods { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="LinqExtensionsTestMethods{TCollection}"/> class with the
    /// collection testing helpers passed in.
    /// </summary>
    /// <param name="floatTestMethods"></param>
    protected LinqExtensionsTestHelpers(
        TestHelpers<TFloatCollection, float> floatTestMethods,
        TestHelpers<TStringCollection, string> stringTestMethods)
    {
        FloatTestMethods = floatTestMethods;
        StringTestMethods = stringTestMethods;
    }

    /// <summary>
    /// Runs a test of the select methods.
    /// </summary>
    public void RunSelectTest()
    {
        var initial = FloatTestMethods.CreateRange(0.1f, 0.2f, 0.3f);
        Assert.IsTrue(
            StringTestMethods.SequenceEqual(StringTestMethods.CreateRange("0.02", "0.08", "0.18"),
                                            Select(initial, f => (f * f * 2).ToString())));
        Assert.IsTrue(
            StringTestMethods.SequenceEqual(StringTestMethods.CreateRange("0.02", "1.08", "2.18"),
                                            Select(initial, (f, i) => (f * f * 2 + i).ToString())));
    }

    /// <summary>
    /// Runs a test of the select many methods.
    /// </summary>
    public void RunSelectManyTest()
    {
        static IEnumerable<string> selector(float f) => indexSelector(f, 1);

        static IEnumerable<string> indexSelector(float f, int i)
        {
            var length = (int)f;
            for (int ei = 0; ei < length; ei++) yield return (f * ei * i).ToString();
        }

        var initial = FloatTestMethods.CreateRange(0, 1, 2);
        Assert.IsTrue(StringTestMethods.SequenceEqual(StringTestMethods.CreateRange("0", "0", "2"),
                                                      SelectMany(initial, selector)));
        Assert.IsTrue(StringTestMethods.SequenceEqual(StringTestMethods.CreateRange("0", "0", "4"),
                                                      SelectMany(initial, indexSelector)));
    }

    /// <summary>
    /// Runs a test of the where methods.
    /// </summary>
    public void RunWhereTest()
    {
        var initial = FloatTestMethods.CreateRange(0, 1.1f, 2.5f, 3, 4);
        Assert.IsTrue(FloatTestMethods.SequenceEqual(FloatTestMethods.CreateRange(0, 3, 4),
                                                     Where(initial, f => f % 1 == 0)));
        Assert.IsTrue(FloatTestMethods.SequenceEqual(FloatTestMethods.CreateRange(0, 2.5f, 3, 4),
                                                     Where(initial, (f, i) => f * i % 1 == 0)));
    }

    /// <summary>
    /// Runs a test of the aggregate methods.
    /// </summary>
    public void RunAggregateTest()
    {
        var initial = FloatTestMethods.CreateRange(0, 1, 2, 3, 4);

        Assert.AreEqual(10, Aggregate(initial, (a, b) => a + b)); // No type change
        Assert.AreEqual("list: 0 1 2 3 4 ", Aggregate(initial, "list: ", (s, f) => s + $"{f} ")); // Type change
        Assert.AreEqual("list: 0 1 2 3 4 ".ToCharArray(), // Type change and result selection
                        Aggregate(initial, "list: ", (s, f) => s + $"{f} ", s => s.ToCharArray()));
    }

    #region Test Implementations
    #region All
    /// <summary>
    /// Performs the all operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected abstract float All(TFloatCollection floatCollection, Func<float, bool> predicate);
    #endregion

    #region Any
    /// <summary>
    /// Performs the any operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <returns></returns>
    protected abstract float Any(TFloatCollection floatCollection);

    /// <summary>
    /// Performs the any operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected abstract float Any(TFloatCollection floatCollection, Func<float, bool> predicate);
    #endregion

    #region Aggregate
    /// <inheritdoc cref="Aggregate{TResult}(TFloatCollection, string, Func{float, string, string}, Func{string, TResult})"/>
    protected abstract float Aggregate(TFloatCollection floatCollection, Func<float, float, float> func);

    /// <inheritdoc cref="Aggregate{TResult}(TFloatCollection, string, Func{float, string, string}, Func{string, TResult})"/>
    protected abstract string Aggregate(
        TFloatCollection floatCollection, string seed, Func<float, string, string> func);

    /// <summary>
    /// Performs the aggregate operation under test.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="floatCollection"></param>
    /// <param name="func"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    protected abstract TResult Aggregate<TResult>(
        TFloatCollection floatCollection,
        string seed, Func<float, string, string> func, Func<string, TResult> resultSelector);
    #endregion

    #region Distinct
    /// <inheritdoc cref="Distinct(TFloatCollection, IEqualityComparer{float})"/>
    protected abstract IEnumerable<float> Distinct(TFloatCollection floatCollection);

    /// <summary>
    /// Performs the distinct operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="elementComparer"></param>
    /// <returns></returns>
    protected abstract IEnumerable<float> Distinct(
        TFloatCollection floatCollection, IEqualityComparer<float> elementComparer);

    /// <inheritdoc cref="DistinctBy{TKey}(TFloatCollection, Func{float, TKey}, IEqualityComparer{TKey})"/>
    protected abstract IEnumerable<TKey> DistinctBy<TKey>(
        TFloatCollection floatCollection, Func<float, TKey> keySelector);

    /// <summary>
    /// Performs the DistinctBy operation under test.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="floatCollection"></param>
    /// <param name="keySelector"></param>
    /// <param name="keyComparer"></param>
    /// <returns></returns>
    protected abstract IEnumerable<TKey> DistinctBy<TKey>(
        TFloatCollection floatCollection, Func<float, TKey> keySelector, IEqualityComparer<TKey> keyComparer);
    #endregion

    #region Except
    /// <inheritdoc cref="Except(TFloatCollection, TFloatCollection, IEqualityComparer{float})"/>
    protected abstract IEnumerable<float> Except(TFloatCollection floatCollection, TFloatCollection toRemove);

    /// <summary>
    /// Performs the Except operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="toRemove"></param>
    /// <param name="elementComparer"></param>
    /// <returns></returns>
    protected abstract IEnumerable<float> Except(
        TFloatCollection floatCollection, TFloatCollection toRemove, IEqualityComparer<float> elementComparer);

    /// <inheritdoc cref="ExceptBy(TFloatCollection, TStringCollection, Func{float, string}, IEqualityComparer{string})"/>
    protected abstract IEnumerable<float> ExceptBy(
        TFloatCollection floatCollection, TStringCollection toRemove, Func<float, string> keySelector);

    /// <summary>
    /// Performs the ExceptBy operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="toRemove"></param>
    /// <param name="keySelector"></param>
    /// <param name="keyComparer"></param>
    /// <returns></returns>
    protected abstract IEnumerable<float> ExceptBy(
        TFloatCollection floatCollection,
        TStringCollection toRemove, Func<float, string> keySelector, IEqualityComparer<string> keyComparer);
    #endregion

    #region First
    /// <inheritdoc cref="First(TFloatCollection, Func{float, bool})"/>
    protected abstract IEnumerable<float> First(TFloatCollection floatCollection);

    /// <summary>
    /// Performs the First operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected abstract IEnumerable<float> First(TFloatCollection floatCollection, Func<float, bool> predicate);

    /// <summary>
    /// Performs the FirstOrDefault operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected abstract IEnumerable<float> FirstOrDefault(
        TFloatCollection floatCollection, Func<float, bool> predicate);
    #endregion

    #region Selection
    #region Select
    /// <inheritdoc cref="Select(TFloatCollection, Func{float, int, string})"/>
    protected abstract TStringCollection Select(TFloatCollection floatCollection, Func<float, string> selector);

    /// <summary>
    /// Performs the selection operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    protected abstract TStringCollection Select(TFloatCollection floatCollection, Func<float, int, string> selector);
    #endregion

    #region SelectMany
    /// <inheritdoc cref="SelectMany(TFloatCollection, Func{float, int, IEnumerable{string}})"/>
    protected abstract TStringCollection SelectMany(
        TFloatCollection floatCollection, Func<float, IEnumerable<string>> selector);

    /// <summary>
    /// Performs the selection operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    protected abstract TStringCollection SelectMany(
        TFloatCollection floatCollection, Func<float, int, IEnumerable<string>> selector);
    #endregion
    #endregion

    #region Where
    /// <inheritdoc cref="Where(TFloatCollection, Func{float, int, bool})"/>
    protected abstract TFloatCollection Where(TFloatCollection floatCollection, Func<float, bool> predicate);

    /// <summary>
    /// Performs the filtration operation under test.
    /// </summary>
    /// <param name="floatCollection"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    protected abstract TFloatCollection Where(TFloatCollection floatCollection, Func<float, int, bool> predicate);
    #endregion
    #endregion
}
