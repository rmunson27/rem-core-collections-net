using Rem.Core.Collections.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Enumeration;

/// <summary>
/// Tests the <see cref="SingletonEnumerator{T}"/> struct.
/// </summary>
[TestClass]
public class SingletonEnumeratorTest
{
    /// <summary>
    /// Tests a <see cref="SingletonEnumerator{T}"/> instance.
    /// </summary>
    [TestMethod]
    public void TestInstance()
    {
        var e = new SingletonEnumerator<int>(3);

        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual(3, e.Current);
        Assert.IsFalse(e.MoveNext());
        Assert.ThrowsException<InvalidOperationException>(() => e.Current);
    }
}
