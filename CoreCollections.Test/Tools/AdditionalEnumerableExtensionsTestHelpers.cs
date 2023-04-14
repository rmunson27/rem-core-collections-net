using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

/// <summary>
/// Testing helpers
/// </summary>
/// <typeparam name="TCollection"></typeparam>
internal abstract class AdditionalEnumerableExtensionsTestHelpers<TCollection> where TCollection : IEnumerable<int>
{
    /// <summary>
    /// Gets test method helpers to use in the implementation of this class.
    /// </summary>
    protected TestHelpers<TCollection, int> TestMethods { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="AdditionalEnumerableExtensionsTestHelpers{TCollection}"/> class
    /// with the test method helper passed in.
    /// </summary>
    /// <param name="testMethods"></param>
    public AdditionalEnumerableExtensionsTestHelpers(TestHelpers<TCollection, int> testMethods)
    {
        TestMethods = testMethods;
    }

    /// <summary>
    /// Runs a test of the operation represented by the <see cref="Cycle(TCollection, int)"/> function.
    /// </summary>
    public void RunCycleTest()
    {
        var items = new[] { 1, 5, 4 };
        var expectedItems = new[] { 1, 5, 4, 1, 5, 4, 1, 5, 4 }; 
        var coll = TestMethods.CreateRange(items);

        Assert.IsTrue(expectedItems.SequenceEqual(Cycle(coll, 3)));
        Assert.IsTrue(items.SequenceEqual(Cycle(coll, 1)));
        Assert.IsFalse(Cycle(coll, 0).Any());
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Cycle(coll, -1));
    }

    /// <summary>
    /// Runs the cycle operation under test.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    protected abstract IEnumerable<int> Cycle(TCollection collection, int count);

    /// <summary>
    /// Runs the cycle forever operation under test.
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    protected abstract IEnumerable<int> CycleForever(TCollection collection);
}
