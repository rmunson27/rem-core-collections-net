using Rem.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;

/// <summary>
/// Tests for the <see cref="ReadOnly2DArray{T}"/> struct and related static functionality.
/// </summary>
[TestClass]
public class ReadOnly2DArrayTest
{
    #region Indexing
    /// <summary>
    /// Tests the <see cref="ReadOnly2DArray{T}"/> indexer.
    /// </summary>
    [TestMethod]
    public void TestIndex()
    {
        var arr = Values.Indices(2, 3);

        for (int r = 0; r < arr.RowCount; r++)
        {
            for (int c = 0; c < arr.ColumnCount; c++)
            {
                Assert.AreEqual((r, c), arr[r, c]);
            }
        }
    }
    #endregion

    #region Vectors
    /// <summary>
    /// Tests the <see cref="ReadOnly2DArray{T}.Rows"/> property.
    /// </summary>
    [TestMethod]
    public void TestRows()
    {
        var arr = Values.Indices(2, 3);
        var expectedRows = new[]
        {
            new[] { (0L, 0L), (0, 1), (0, 2) },
            new[] { (1L, 0L), (1, 1), (1, 2) },
        };

        // Test enumeration
        Assert.That.AreSequenceEqual(expectedRows, arr.Rows, IndexSequenceComparer);

        // Test indexing
        Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Rows[-1]);
        Assert.That.AreSequenceEqual(expectedRows[0], arr.Rows[0]);
        Assert.That.AreSequenceEqual(expectedRows[1], arr.Rows[1]);
        Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Rows[2]);

        // Test row indexing
        for (int i = 0; i < 2; i++)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Rows[i][-1]);
            Assert.AreEqual(expectedRows[i][0], arr.Rows[i][0]);
            Assert.AreEqual(expectedRows[i][1], arr.Rows[i][1]);
            Assert.AreEqual(expectedRows[i][2], arr.Rows[i][2]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Rows[i][3]);
        }
    }

    /// <summary>
    /// Tests the <see cref="ReadOnly2DArray{T}.Columns"/> method.
    /// </summary>
    [TestMethod]
    public void TestColumns()
    {
        var arr = Values.Indices(2, 3);
        var expectedColumns = new[]
        {
            new[] { (0L, 0L), (1, 0) },
            new[] { (0L, 1L), (1, 1) },
            new[] { (0L, 2L), (1, 2) },
        };

        // Test enumeration
        Assert.That.AreSequenceEqual(expectedColumns, arr.Columns, IndexSequenceComparer);

        // Test indexing
        Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Columns[-1]);
        Assert.That.AreSequenceEqual(expectedColumns[0], arr.Columns[0]);
        Assert.That.AreSequenceEqual(expectedColumns[1], arr.Columns[1]);
        Assert.That.AreSequenceEqual(expectedColumns[2], arr.Columns[2]);
        Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Columns[3]);

        // Test column indexing
        for (int i = 0; i < 3; i++)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Columns[i][-1]);
            Assert.AreEqual(expectedColumns[i][0], arr.Columns[i][0]);
            Assert.AreEqual(expectedColumns[i][1], arr.Columns[i][1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.Columns[i][2]);
        }
    }
    #endregion

    #region Clone
    /// <summary>
    /// Tests the <see cref="ReadOnly2DArray.Clone{T}(T[,])"/> method.
    /// </summary>
    [TestMethod]
    public void TestClone()
    {
        var arr = new[,] { { 1, 2 }, { 3, 4 } };
        ReadOnly2DArray<int> readonly1 = new(arr), readonly2 = new(arr);
        ReadOnly2DArray<int> readonlyClone = ReadOnly2DArray.Clone(arr);

        // Control test
        Assert.AreEqual(readonly1, readonly2);

        // Ensure the clone is different
        Assert.AreNotEqual(readonlyClone, readonly1);
    }
    #endregion

    #region Count
    /// <summary>
    /// Tests the count-getting methods.
    /// </summary>
    [TestMethod]
    public void TestCount()
    {
        var arr = Values.Indices(2, 3);
        Assert.AreEqual(6, arr.Count);
        Assert.AreEqual(6L, arr.LongCount);
    }

    /// <summary>
    /// Tests the single dimension count-getting methods.
    /// </summary>
    [TestMethod]
    public void TestDimensionCount()
    {
        var arr = Values.Indices(6, 5);
        Assert.AreEqual(6, arr.RowCount);
        Assert.AreEqual(6L, arr.LongRowCount);
        Assert.AreEqual(5, arr.ColumnCount);
        Assert.AreEqual(5L, arr.LongColumnCount);
    }
    #endregion

    #region Helpers
    /// <summary>
    /// An equality comparer for comparing the equality of sequences of indices.
    /// </summary>
    private readonly NestedEqualityComparer<IEnumerable<(long, long)>, (long, long)> IndexSequenceComparer
        = SequenceEquality.EnumerableComparer<(long, long)>();
    #endregion
}
