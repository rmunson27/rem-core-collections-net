using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;

/// <summary>
/// Tests for the <see cref="Readonly2DArray{T}"/> struct and related static functionality.
/// </summary>
[TestClass]
public class Readonly2DArrayTest
{
    /// <summary>
    /// Tests the <see cref="Readonly2DArray{T}"/> indexer.
    /// </summary>
    [TestMethod]
    public void TestIndex()
    {
        var arr = Values.Indices(2, 3);

        for (int r = 0; r < arr.GetLength(0); r++)
        {
            for (int s = 0; s < arr.GetLength(1); s++)
            {
                Assert.AreEqual((r, s), arr[r, s]);
            }
        }
    }

    /// <summary>
    /// Tests the <see cref="Readonly2DArray.Clone{T}(T[,])"/> method.
    /// </summary>
    [TestMethod]
    public void TestClone()
    {
        var arr = new[,] { { 1, 2 }, { 3, 4 } };
        Readonly2DArray<int> readonly1 = new(arr), readonly2 = new(arr);
        Readonly2DArray<int> readonlyClone = Readonly2DArray.Clone(arr);

        // Control test
        Assert.AreEqual(readonly1, readonly2);

        // Ensure the clone is different
        Assert.AreNotEqual(readonlyClone, readonly1);
    }

    /// <summary>
    /// Tests the length-getting methods.
    /// </summary>
    [TestMethod]
    public void TestLength()
    {
        var arr = Values.Indices(2, 3);
        Assert.AreEqual(6, arr.Length);
        Assert.AreEqual(6L, arr.LongLength);
    }

    /// <summary>
    /// Tests the single dimension length-getting methods.
    /// </summary>
    [TestMethod]
    public void TestGetLength()
    {
        var arr = Values.Indices(6, 5);
        Assert.AreEqual(6, arr.GetLength(0));
        Assert.AreEqual(6L, arr.GetLongLength(0));
        Assert.AreEqual(5, arr.GetLength(1));
        Assert.AreEqual(5L, arr.GetLongLength(1));
    }
}
