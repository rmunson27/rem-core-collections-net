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
    /// Tests truncation of array slices.
    /// </summary>
    [TestMethod]
    public void TestTruncate()
    {
        var middleFour = ArraySlice.Create(FirstTen, 3, 4);
        Assert.IsTrue(middleFour.Truncate(2).SequenceEqual(new[] { 3, 4 }));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => middleFour.Truncate(5));
    }
}
