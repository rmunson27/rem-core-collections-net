using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests the <see cref="LongIndex"/> class functionality.
/// </summary>
[TestClass]
public class LongIndexTest
{
    private const long TestCollectionLength = 4;

    private static readonly ImmutableArray<(LongIndex Index, long Offset)> ValidOffsetIndices
        = new (LongIndex, long)[]
        {
            (0, 0),
            (1, 1), (^1, 3),
            (2, 2), (^2, 2),
            (3, 3), (^3, 1),
            (^4, 0),
        }.ToImmutableArray();

    private static readonly ImmutableArray<LongIndex> EndOfCollectionIndices
        = new LongIndex[] { 4, ^0 }.ToImmutableArray();

    private static readonly LongIndex TooLargeOffsetIndex = 5;
    private static readonly LongIndex TooSmallOffsetIndex = ^5;

    /// <summary>
    /// Tests the <see cref="LongIndex.GetOffset(long, bool)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetOffset()
    {
        // All in range
        foreach (var (index, offset) in ValidOffsetIndices)
        {
            Assert.AreEqual(offset, index.GetOffset(TestCollectionLength),
                            $"Index {index} offset was not expected value.");
        }

        // Should be in range as long as the collection length is allowed
        foreach (var index in EndOfCollectionIndices)
        {
            Assert.AreEqual(TestCollectionLength, index.GetOffset(TestCollectionLength, allowCollectionLength: true),
                            $"Index {index} offset was not expected value.");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => index.GetOffset(TestCollectionLength, allowCollectionLength: false),
                $"Index {index} offset computation did not fail.");
        }

        // Should be out of range even if the collection length is allowed
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => TooLargeOffsetIndex.GetOffset(TestCollectionLength, allowCollectionLength: true));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => TooSmallOffsetIndex.GetOffset(TestCollectionLength, allowCollectionLength: true));
    }

    /// <summary>
    /// Tests the <see cref="LongIndex.TryGetUnclampedOffset(long, out long, bool)"/> method.
    /// </summary>
    [TestMethod]
    public void TestTryGetUnclampedOffset()
    {
        long computedOffset;

        // All in range
        foreach (var (index, offset) in ValidOffsetIndices)
        {
            Assert.IsTrue(index.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                      allowCollectionLength: true),
                          $"Index {index} offset was unclamped.");
            Assert.AreEqual(offset, computedOffset, $"Index {index} clamped offset did not match expected value.");

            Assert.IsTrue(index.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                      allowCollectionLength: false),
                          $"Index {index} offset was unclamped.");
            Assert.AreEqual(offset, computedOffset, $"Index {index} clamped offset did not match expected value.");
        }

        // Should be in range as long as the collection length is allowed
        foreach (var index in EndOfCollectionIndices)
        {
            Assert.IsTrue(index.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                      allowCollectionLength: true),
                          $"Index {index} offset was unclamped.");
            Assert.AreEqual(TestCollectionLength, computedOffset,
                            $"Index {index} clamped offset did not match expected value.");

            Assert.IsFalse(index.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                      allowCollectionLength: false),
                          $"Index {index} offset was clamped.");
            Assert.AreEqual(TestCollectionLength - 1, computedOffset,
                            $"Index {index} unclamped offset did not match expected value.");
        }

        // Should be out of range even if the collection length is allowed
        Assert.IsFalse(TooLargeOffsetIndex.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                                 allowCollectionLength: true));
        Assert.AreEqual(TestCollectionLength, computedOffset);
        Assert.IsFalse(TooLargeOffsetIndex.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                                 allowCollectionLength: false));
        Assert.AreEqual(TestCollectionLength - 1, computedOffset);

        Assert.IsFalse(TooSmallOffsetIndex.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                                 allowCollectionLength: true));
        Assert.AreEqual(0, computedOffset);
        Assert.IsFalse(TooSmallOffsetIndex.TryGetUnclampedOffset(TestCollectionLength, out computedOffset,
                                                                 allowCollectionLength: false));
        Assert.AreEqual(0, computedOffset);
    }

    /// <summary>
    /// Tests the <see cref="LongIndex.GetClampedOffset(long, bool)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetClampedOffset()
    {
        // All in range
        foreach (var (index, offset) in ValidOffsetIndices)
        {
            Assert.AreEqual(offset, index.GetClampedOffset(TestCollectionLength),
                            $"Index {index} offset was not expected value.");
        }

        // Should be in range as long as the collection length is allowed
        foreach (var index in EndOfCollectionIndices)
        {
            Assert.AreEqual(TestCollectionLength,
                            index.GetClampedOffset(TestCollectionLength, allowCollectionLength: true),
                            $"Index {index} offset was not expected value.");
            Assert.AreEqual(TestCollectionLength - 1,
                            index.GetClampedOffset(TestCollectionLength, allowCollectionLength: false),
                            $"Index {index} offset was not expected value.");
        }

        // Should be out of range even if the collection length is allowed
        Assert.AreEqual(TestCollectionLength,
                        TooLargeOffsetIndex.GetClampedOffset(TestCollectionLength, allowCollectionLength: true));
        Assert.AreEqual(TestCollectionLength - 1,
                        TooLargeOffsetIndex.GetClampedOffset(TestCollectionLength, allowCollectionLength: false));
        Assert.AreEqual(0, TooSmallOffsetIndex.GetClampedOffset(TestCollectionLength));
    }

    /// <summary>
    /// Tests implicit conversions from <see cref="long"/> and <see cref="Index"/> instances to
    /// <see cref="LongIndex"/> instances.
    /// </summary>
    [TestMethod]
    public void TestConversion()
    {
        Assert.AreEqual(new LongIndex(3, IsFromEnd: false), 3);
        Assert.AreEqual(new LongIndex(3, IsFromEnd: true), ^3);
    }

    /// <summary>
    /// Tests that the behavior of <see cref="Index.GetOffset(int)"/> matches the behavior
    /// of <see cref="LongIndex.GetOffset(long, bool)"/> (when the collection length is allowed).
    /// </summary>
    [TestMethod]
    public void TestGetOffsetComparison()
    {
        foreach (var (index, _) in ValidOffsetIndices)
        {
            Assert.AreEqual(((Index)index).GetOffset((int)TestCollectionLength),
                            index.GetOffset(TestCollectionLength, allowCollectionLength: true),
                            $"Index {index} offset did not match System.Index offset.");
        }
    }
}
