using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests of the <see cref="SelectedList{T}"/> class and the derived <see cref="FuncSelectedList{T}"/> classes.
/// </summary>
[TestClass]
public class SelectedListTest
{
    #region Tests
    /// <summary>
    /// Tests sequence equality of <see cref="SelectedList{T}"/> instances.
    /// </summary>
    [TestMethod]
    public void TestSequenceEqual()
    {
        var list = new FuncSelectedList<int>(Square, 4);
        var list2 = new FuncSelectedList<int>(Square, 4);
        Assert.IsTrue(list.SequenceEqual(list));
        Assert.IsTrue(list2.SequenceEqual(list));
        Assert.IsTrue(list.SequenceEqual(new[] { 0, 1, 4, 9 }));
    }

    /// <summary>
    /// Tests hash code getting methods of <see cref="SelectedList{T}"/> instances.
    /// </summary>
    [TestMethod]
    public void TestGetHashCode()
    {
        var list = new FuncSelectedList<int>(Square, 4);
        Assert.AreEqual(list.GetSequenceHashCode(), SequenceEquality.GetHashCode(new[] { 0, 1, 4, 9 }));
    }

    /// <summary>
    /// Tests the indexer of the <see cref="SelectedList{T}"/> class.
    /// </summary>
    [TestMethod]
    public void TestIndexer()
    {
        var list = new FuncSelectedList<int>(Square, 4);
        Assert.ThrowsException<IndexOutOfRangeException>(() => list[-1]);
        for (int i = 0; i < list.Count; i++)
        {
            Assert.AreEqual(Square(i), list[i]);
        }
        Assert.ThrowsException<IndexOutOfRangeException>(() => list[list.Count]);
    }

    /// <summary>
    /// Tests the <see cref="SelectedList{T}.Enumerator"/> struct.
    /// </summary>
    [TestMethod]
    public void TestEnumerator()
    {
        var list = new FuncSelectedList<int>(Square, 4);
        var enumerator = list.GetEnumerator();

        // Run a single test of the enumeration to ensure that resetting the enumerator works as expected
        void testEnumeration()
        {
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(i * i, enumerator.Current);
            }
            Assert.IsFalse(enumerator.MoveNext());
            Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
        }
        testEnumeration();
        enumerator.Reset();
        testEnumeration();
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Squares the argument passed in.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private static int Square(int i) => i * i;
    #endregion
}
