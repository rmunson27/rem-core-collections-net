using Rem.Core.Collections.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Enumeration;

/// <summary>
/// Tests for the <see cref="MaybeEnumerator{T}"/> struct.
/// </summary>
[TestClass]
public class MaybeEnumeratorTest
{
    /// <summary>
    /// Tests an empty <see cref="MaybeEnumerator{T}"/>.
    /// </summary>
    [TestMethod]
    public void TestEmpty()
    {
        var e = new MaybeEnumerator<int>();

        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
        Assert.IsFalse(e.MoveNext());
        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
    }

    /// <summary>
    /// Tests a <see cref="MaybeEnumerator{T}"/> wrapping a single element.
    /// </summary>
    [TestMethod]
    public void TestSingleElement()
    {
        var e = new MaybeEnumerator<int>(4);

        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual(4, e.Current);
        Assert.IsFalse(e.MoveNext());
        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
    }
}
