using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Test of the <see cref="Arrays"/> class functionality.
/// </summary>
[TestClass]
public class ArraysTest
{
    /// <summary>
    /// Tests the <see cref="Arrays.Create{T}(long, T)"/> method.
    /// </summary>
    [TestMethod]
    public void TestCreate()
    {
        Assert.IsTrue(new[] { 1, 1, 1 }.SequenceEqual(Arrays.Create(3, 1)));
    }

    /// <summary>
    /// Tests the <see cref="Arrays.CreateLazy{T}(long, Func{T})"/> method and all overloads.
    /// </summary>
    [TestMethod]
    public void TestCreateLazy()
    {
        var count = 0;

        int GetCountThenIncrement() => count++;
        int GetIntIndexTimesCountThenIncrement(int i) => i * count++;
        long GetLongIndexTimesCountThenIncrement(long i) => i * count++;

        Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(Arrays.CreateLazy(10, GetCountThenIncrement)));
        count = 0;
        Assert.IsTrue(Enumerable.Range(0, 10).Select(x => x * x)
                                .SequenceEqual(Arrays.CreateLazy(10, GetIntIndexTimesCountThenIncrement)));
        count = 0;
        Assert.IsTrue(Enumerable.Range(0, 10).Select(x => (long)x * x)
                                .SequenceEqual(Arrays.CreateLazy(10L, GetLongIndexTimesCountThenIncrement)));
    }

    /// <summary>
    /// Tests the <see cref="Arrays.SelectArray{T, U}(T[], Func{T, U})"/> extension method.
    /// </summary>
    [TestMethod]
    public void TestSelectArray()
    {
        var firstInts = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var firstSquares = new[] { 0, 1, 4, 9, 16, 25, 36, 49, 64, 81 };

        Assert.IsTrue(firstSquares.SequenceEqual(firstInts.SelectArray(x => x * x)));
    }
}
