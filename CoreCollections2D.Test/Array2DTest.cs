using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;

/// <summary>
/// Tests of the <see cref="Array2D"/> class functionality.
/// </summary>
[TestClass]
public class Array2DTest
{
    /// <summary>
    /// Tests the <see cref="Array2D.FromRows{T}"/> method overloads.
    /// </summary>
    [TestMethod]
    public void TestFromRows()
    {
        var rows = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 } };
        var expectedArray = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromRows(rows)));
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromRows(rows as IReadOnlyList<int[]>)));
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromRows(rows as IReadOnlyList<int>[])));
        Assert.IsTrue(
            SequenceEquality2D.Equals(expectedArray, Array2D.FromRows(rows as IReadOnlyList<IReadOnlyList<int>>)));
    }

    /// <summary>
    /// Tests the <see cref="Array2D.FromColumns{T}"/> method overloads.
    /// </summary>
    [TestMethod]
    public void TestFromColumns()
    {
        var columns = new[] { new[] { 1, 4 }, new[] { 2, 5 }, new[] { 3, 6 } };
        var expectedArray = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromColumns(columns)));
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromColumns(columns as IReadOnlyList<int[]>)));
        Assert.IsTrue(SequenceEquality2D.Equals(expectedArray, Array2D.FromColumns(columns as IReadOnlyList<int>[])));
        Assert.IsTrue(
            SequenceEquality2D.Equals(
                expectedArray, Array2D.FromColumns(columns as IReadOnlyList<IReadOnlyList<int>>)));
    }
}
