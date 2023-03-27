using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

/// <summary>
/// Controls testing of methods that compare instances of two collections of differing given types using
/// sequence equality.
/// </summary>
/// <typeparam name="TLeftCollection">The collection on the left-hand side of the equality operation.</typeparam>
/// <typeparam name="TRightCollection">The collection on the right-hand side of the equality operation.</typeparam>
internal abstract class SequenceEqualityTestHelpers<TLeftCollection, TRightCollection>
    where TLeftCollection : notnull, IEnumerable<float>
    where TRightCollection : notnull, IEnumerable<float>
{
    /// <summary>
    /// Gets an object that contains helper logic for working with <typeparamref name="TLeftCollection"/>.
    /// </summary>
    protected TestHelpers<TLeftCollection, float> LeftHelpers { get; }

    /// <summary>
    /// Gets an object that contains helper logic for working with <typeparamref name="TRightCollection"/>.
    /// </summary>
    protected TestHelpers<TRightCollection, float> RightHelpers { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="SequenceEqualityTestHelpers{TLeftCollection, TRightCollection}"/>
    /// class with the specified helper objects.
    /// </summary>
    /// <param name="leftHelpers"></param>
    /// <param name="rightHelpers"></param>
    protected SequenceEqualityTestHelpers(
        TestHelpers<TLeftCollection, float> leftHelpers, TestHelpers<TRightCollection, float> rightHelpers)
    {
        LeftHelpers = leftHelpers;
        RightHelpers = rightHelpers;
    }

    /// <summary>
    /// Runs a sequence equality test of <typeparamref name="TLeftCollection"/> and
    /// <typeparamref name="TRightCollection"/> instances using
    /// <see cref="SequenceEqual(TLeftCollection, TRightCollection)"/>.
    /// </summary>
    public void RunEqualsTest()
    {
        var leftColl = LeftHelpers.CreateRange(1, 2, 3);
        Assert.IsTrue(SequenceEqual(leftColl, RightHelpers.CreateRange(1, 2, 3)));
        Assert.IsFalse(SequenceEqual(leftColl, RightHelpers.CreateRange(1, 3, 2)));
        Assert.IsFalse(SequenceEqual(leftColl, RightHelpers.CreateRange(1, 2, 3, 4)));
    }

    /// <summary>
    /// Compares the supplied <typeparamref name="TLeftCollection"/> and <see cref="TRightCollection"/> instances
    /// passed in for sequence equality.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    protected abstract bool SequenceEqual(
        [DisallowDefault] TLeftCollection lhs, [DisallowDefault] TRightCollection rhs);
}

/// <summary>
/// Controls testing of methods that compare instances of a collection of a given type using sequence equality.
/// </summary>
/// <typeparam name="TCollection"></typeparam>
internal abstract class SequenceEqualityTestHelpers<TCollection> where TCollection : notnull, IEnumerable<float>
{
    /// <summary>
    /// Gets an object that contains helper logic for working with <typeparamref name="TCollection"/>.
    /// </summary>
    protected TestHelpers<TCollection, float> Helpers { get; }

    /// <summary>
    /// Gets the comparer that will be tested.
    /// </summary>
    protected abstract INestedEqualityComparer<TCollection, float> Comparer { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="SequenceEqualityTestHelpers{TCollection}"/> class with the test
    /// method helper object passed in.
    /// </summary>
    /// <param name="testMethods"></param>
    protected SequenceEqualityTestHelpers(TestHelpers<TCollection, float> testMethods)
    {
        Helpers = testMethods;
    }

    #region Test Methods
    /// <summary>
    /// Runs a sequence equality test of <typeparamref name="TCollection"/> instances using
    /// <see cref="SequenceEqual(TCollection, TCollection)"/>.
    /// </summary>
    public void RunEqualsTest()
    {
        var coll = Helpers.CreateRange(1, 2, 3);
        Assert.IsTrue(Helpers.SequenceEqual(coll, Helpers.CreateRange(1, 2, 3)));
        Assert.IsFalse(Helpers.SequenceEqual(coll, Helpers.CreateRange(1, 3, 2)));
        Assert.IsFalse(Helpers.SequenceEqual(coll, Helpers.CreateRange(1, 2, 3, 4)));
    }

    /// <summary>
    /// Runs a sequence hash code test of <typeparamref name="TCollection"/> instances using
    /// <see cref="GetSequenceHashCode(TCollection)"/>.
    /// </summary>
    public void RunGetHashCodeTest()
    {
        var expected = new float[] { 1, 2, 3 }.GetSequenceHashCode();
        Assert.AreEqual(expected, GetSequenceHashCode(Helpers.CreateRange(1, 2, 3)));
        Assert.AreNotEqual(expected, GetSequenceHashCode(Helpers.CreateRange(1, 3, 2))); // Wrong order
        Assert.AreNotEqual(expected, GetSequenceHashCode(Helpers.CreateRange(1, 2, 3, 4))); // Extra element
    }

    #region Comparer
    /// <summary>
    /// Runs a sequence equality test of <typeparamref name="TCollection"/> instances using the nested equality
    /// comparer under test.
    /// </summary>
    public void RunComparerEqualsTest()
    {
        var coll = Helpers.CreateRange(1, 2, 3);

        // Non-default testing
        Assert.IsTrue(Comparer.Equals(coll, Helpers.CreateRange(1, 2, 3)));
        Assert.IsFalse(Comparer.Equals(coll, Helpers.CreateRange(1, 3, 2))); // Order is wrong
        Assert.IsFalse(Comparer.Equals(coll, Helpers.CreateRange(1, 2, 3, 4))); // Extra element

        // Default testing
        Assert.IsTrue(Comparer.Equals(default, default));
        Assert.IsFalse(Comparer.Equals(coll, default));
        Assert.IsFalse(Comparer.Equals(default, coll));
    }

    /// <summary>
    /// Runs a sequence hash code test of <typeparamref name="TCollection"/> instances using the nested equality
    /// comparer under test.
    /// </summary>
    public void RunComparerGetHashCodeTest()
    {
        var expected = new float[] { 1, 2, 3 }.GetSequenceHashCode();
        Assert.AreEqual(expected, Comparer.GetHashCode(Helpers.CreateRange(1, 2, 3)));
        Assert.AreNotEqual(expected, Comparer.GetHashCode(Helpers.CreateRange(1, 3, 2))); // Wrong order
        Assert.AreNotEqual(expected, Comparer.GetHashCode(Helpers.CreateRange(1, 2, 3, 4))); // Extra element
    }
    #endregion
    #endregion

    /// <summary>
    /// Gets a sequence-based hash code for the supplied <typeparamref name="TCollection"/> instance.
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    protected abstract int GetSequenceHashCode([DisallowDefault] TCollection collection);
}
