using Rem.Core.Collections.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Enumeration;

/// <summary>
/// Tests the <see cref="ArrayEnumerator{TElement}"/> struct.
/// </summary>
[TestClass]
public class ArrayEnumeratorTest
{
    /// <summary>
    /// Tests an empty instance of the <see cref="ArrayEnumerator{TElement}"/> struct.
    /// </summary>
    [TestMethod]
    public void TestEmpty()
    {
        var e = new ArrayEnumerator<int>(Array.Empty<int>());

        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
        Assert.IsFalse(e.MoveNext());
        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
    }

    /// <summary>
    /// Tests a non-empty instance of the <see cref="ArrayEnumerator{TElement}"/> struct.
    /// </summary>
    [TestMethod]
    public void TestNonEmpty()
    {
        var arr = new[] { 1, 2 };
        var e = new ArrayEnumerator<int>(arr);

        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual(1, e.Current);
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual(2, e.Current);
        Assert.IsFalse(e.MoveNext());
        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
    }
}
