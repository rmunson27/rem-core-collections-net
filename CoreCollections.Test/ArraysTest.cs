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
