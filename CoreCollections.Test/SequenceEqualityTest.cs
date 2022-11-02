using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests of the <see cref="SequenceEquality"/> class.
/// </summary>
[TestClass]
public class SequenceEqualityTest
{
    /// <summary>
    /// Tests the <see cref="SequenceEquality.GetHashCode{T}(IEnumerable{T}, IEqualityComparer{T}?)"/> and
    /// <see cref="SequenceEquality.GetHashCode{T}(ImmutableArray{T}, IEqualityComparer{T}?)"/>
    /// methods.
    /// </summary>
    [TestMethod]
    public void TestGetHashCode()
    {
        var arr = new[] { 1, 2, 3 };
        var list = new List<int> { 1, 2, 3 };
        var immutableArr = list.ToImmutableArray();
        var expectedHashCode = SequenceEquality.GetHashCode(arr);

        Assert.AreEqual(expectedHashCode, SequenceEquality.GetHashCode(list));
        Assert.AreEqual(expectedHashCode, SequenceEquality.GetHashCode(immutableArr));
    }

    /// <summary>
    /// Tests the <see cref="SequenceEquality.EnumerableComparer{T}"/> method.
    /// </summary>
    [TestMethod]
    public void TestEnumerableComparer()
    {
        var arr = new[] { 1, 2, 3 };
        var list = new List<int> { 5, 6, 7 };
        var otherParityList = new List<int> { 6, 7, 8 };

        var comparer = SequenceEquality.EnumerableComparer<int>();
        Assert.IsTrue(comparer.Equals(arr, list, IntParityComparer));
        Assert.IsFalse(comparer.Equals(arr, otherParityList, IntParityComparer));
        Assert.AreEqual(comparer.GetHashCode(arr, IntParityComparer), comparer.GetHashCode(list, IntParityComparer));
        Assert.AreNotEqual(
            comparer.GetHashCode(arr, IntParityComparer), comparer.GetHashCode(otherParityList, IntParityComparer));
    }

    /// <summary>
    /// Tests the <see cref="SequenceEquality.ImmutableArrayComparer{T}"/> method.
    /// </summary>
    [TestMethod]
    public void TestImmutableArrayComparer()
    {
        var arr = ImmutableArray.CreateRange(new[] { 9, 10, 11 });
        var arr2 = ImmutableArray.CreateRange(new[] { 11, 12, 13 });
        var otherParityArr = ImmutableArray.CreateRange(new[] { 10, 11, 12 });

        var comparer = SequenceEquality.ImmutableArrayComparer<int>();
        Assert.IsTrue(comparer.Equals(arr, arr2, IntParityComparer));
        Assert.IsFalse(comparer.Equals(arr, otherParityArr, IntParityComparer));
        Assert.AreEqual(comparer.GetHashCode(arr, IntParityComparer), comparer.GetHashCode(arr2, IntParityComparer));
        Assert.AreNotEqual(
            comparer.GetHashCode(arr, IntParityComparer), comparer.GetHashCode(otherParityArr, IntParityComparer));
    }

    private static readonly EqualityComparer<int> IntParityComparer = new IntParityComparerType();

    private sealed class IntParityComparerType : EqualityComparer<int>
    {
        public override bool Equals(int x, int y)
        {
            return IsEven(x) == IsEven(y);
        }

        public override int GetHashCode([DisallowNull] int obj)
        {
            return IsEven(obj).GetHashCode();
        }

        private static bool IsEven(int i) => i % 2 == 0;
    }
}
