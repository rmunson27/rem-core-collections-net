using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests the <see cref="ArraySlice{T}"/> and <see cref="ReadOnlyArraySlice{T}"/> type functionality.
/// </summary>
/// <remarks>
/// Since both have the same core functionality (provided by the same internal type), only the
/// <see cref="ArraySlice{T}"/> type will be tested in order to simplify things.
/// </remarks>
[TestClass]
public class ArraySlicesTest
{
    private readonly int[] FirstTen = Enumerable.Range(0, 10).ToArray();

    /// <summary>
    /// Tests the <see cref="ArraySlice{T}.IsDefault"/> property.
    /// </summary>
    [TestMethod]
    public void TestIsDefault()
    {
        Assert.IsTrue(default(ArraySlice<int>).IsDefault);
        Assert.IsFalse(new ArraySlice<int>(new[] { 1, 2, 3 }).IsDefault);
    }

    /// <summary>
    /// Tests conversions to and from <see cref="ArraySlice{T}"/> instances.
    /// </summary>
    [TestMethod]
    public void TestConversions()
    {
        var arr = new[] { 1, 2, 3, 4, 5 };
        var seg = new ArraySegment<int>(arr, 2, 2);
        var slice = ArraySlice.Create(arr, 2, 2);

        Assert.IsTrue(((ArraySlice<int>)arr).SequenceEqual(arr));
        Assert.IsTrue(((ArraySlice<int>)seg).SequenceEqual(seg));
        Assert.IsTrue(((ArraySegment<int>)slice).SequenceEqual(seg));
        Assert.IsTrue(((ReadOnlySpan<int>)seg).ToArray().SequenceEqual(seg)); // Control
        Assert.IsTrue(((ReadOnlySpan<int>)slice).ToArray().SequenceEqual(seg));
        Assert.IsTrue(((Span<int>)slice).ToArray().SequenceEqual(seg));
    }

    /// <summary>
    /// Tests enumeration of <see cref="ArraySlice{T}"/> instances.
    /// </summary>
    [TestMethod]
    public void TestEnumeration()
    {
        // Need to ensure that the struct enumerators are the same
        static IEnumerable<T> EnumerateStructEnumerator<T>(ArraySlice<T> slice)
        {
            foreach (var value in slice) yield return value;
        }

        ArraySlice<int> firstTenSlice = new(FirstTen), emptySlice = new(Array.Empty<int>()),
                        secondFiveSlice = new(FirstTen, 5), thirdTwoSlice = new(FirstTen, 4, 2);

        // Object enumerators
        Assert.IsTrue(firstTenSlice.SequenceEqual(FirstTen));
        Assert.IsTrue(emptySlice.SequenceEqual(Array.Empty<int>()));
        Assert.IsTrue(secondFiveSlice.SequenceEqual(Enumerable.Range(5, 5)));
        Assert.IsTrue(thirdTwoSlice.SequenceEqual(Enumerable.Range(4, 2)));

        // Struct enumerators
        Assert.IsTrue(EnumerateStructEnumerator(firstTenSlice).SequenceEqual(FirstTen));
        Assert.IsTrue(EnumerateStructEnumerator(emptySlice).SequenceEqual(Array.Empty<int>()));
        Assert.IsTrue(EnumerateStructEnumerator(secondFiveSlice).SequenceEqual(Enumerable.Range(5, 5)));
        Assert.IsTrue(EnumerateStructEnumerator(thirdTwoSlice).SequenceEqual(Enumerable.Range(4, 2)));
    }

    /// <summary>
    /// Tests indexing of array slices.
    /// </summary>
    [TestMethod]
    public void TestIndexing()
    {
        var firstTen = FirstTen.ToArray(); // Copy so does not change the static value

        var secondThreeSlice = ArraySlice.Create(firstTen, 3, 3);
        Assert.AreEqual(4, secondThreeSlice[1]);
        Assert.AreEqual(5, secondThreeSlice[^1]);

        secondThreeSlice[1] = 1;
        secondThreeSlice[^1] = 2;
        Assert.AreEqual(1, secondThreeSlice[1]);
        Assert.AreEqual(2, secondThreeSlice[^1]);

        Assert.IsTrue(firstTen[1..^1][1..^1].SequenceEqual(ArraySlice.Create(firstTen)[1..^1][1..^1]));
    }

    /// <summary>
    /// Tests re-slicing of array slices.
    /// </summary>
    [TestMethod]
    public void TestSlice()
    {
        var middleFour = ArraySlice.Create(FirstTen, 3, 4);
        Assert.IsTrue(middleFour.Slice(2).SequenceEqual(new[] { 5, 6 }));
        Assert.IsTrue(middleFour.Slice(1, 2).SequenceEqual(new[] { 4, 5 }));
        Assert.IsTrue(middleFour.Slice(1..^1).SequenceEqual(new[] { 4, 5 }));
        Assert.IsTrue(middleFour.Slice((LongRange)(1..^1)).SequenceEqual(new[] { 4, 5 }));

        Assert.IsTrue(Array.Empty<int>().SequenceEqual(middleFour.Slice(0..0)));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => middleFour.Slice(6));
        Assert.ThrowsException<ArgumentException>(() => middleFour.Slice(0..8));
        Assert.ThrowsException<ArgumentException>(() => middleFour.Slice(^8..3));
    }

    /// <summary>
    /// Tests truncation of array slices.
    /// </summary>
    [TestMethod]
    public void TestTruncateAt()
    {
        var middleFour = ArraySlice.Create(FirstTen, 3, 4);
        Assert.IsTrue(middleFour.TruncateAt(2).SequenceEqual(new[] { 3, 4 }));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => middleFour.TruncateAt(5));
    }
}
