using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests the <see cref="LongRange"/> struct.
/// </summary>
[TestClass]
public class LongRangeTest
{
    /// <summary>
    /// The length of the collection that is used in testing.
    /// </summary>
    private const long TestCollectionLength = 4;

    /// <summary>
    /// Ranges for which the offset and length can be obtained for a collection of length
    /// <see cref="TestCollectionLength"/> via a call to <see cref="LongRange.GetOffsetAndLength(long)"/>.
    /// </summary>
    private static readonly ImmutableArray<(LongRange Range, long Offset, long Count)> ValidRanges
        = new (LongRange, long, long)[]
        {
            (0..4, 0, 4), (^4..4, 0, 4), (0..^0, 0, 4), (^4..^0, 0, 4), // Full range

            (1..4, 1, 3), (^3..4, 1, 3), (1..^0, 1, 3), (^3..^0, 1, 3), // Includes end
            (0..3, 0, 3), (^4..3, 0, 3), (0..^1, 0, 3), (^4..^1, 0, 3), // Includes start

            (1..3, 1, 2), (^3..3, 1, 2), (1..^1, 1, 2), (^3..^1, 1, 2), // Strictly in the middle

            (2..2, 2, 0), (2..^2, 2, 0), (^2..2, 2, 0), (^2..^2, 2, 0), // Empty
        }.ToImmutableArray();

    /// <summary>
    /// Ranges for which the offset can be obtained for a collection of length <see cref="TestCollectionLength"/>
    /// via a call to <see cref="LongRange.GetOffset(long)"/>, but the length cannot be similarly accessed via a call
    /// to <see cref="LongRange.GetLength(long)"/>, and rather must be retrieved clamped via a call to
    /// <see cref="LongRange.GetClampedLength(long)"/>.
    /// </summary>
    private static readonly ImmutableArray<(LongRange Range, long Offset, long ClampedLength)> ValidOffsetClampableLengthRanges
        = new (LongRange, long, long)[]
        {
            (3..5, 3, 1), (^1..5, 3, 1),
            (4..6, 4, 0), (^0..6, 4, 0),
        }.ToImmutableArray();

    /// <summary>
    /// Ranges for which the offset and length cannot be obtained for a collection of length
    /// <see cref="TestCollectionLength"/> via a call to <see cref="LongRange.GetOffsetAndLength(long)"/>,
    /// but can be obtained via a call to <see cref="LongRange.GetClampedOffsetAndLength(long)"/>.
    /// </summary>
    private static readonly ImmutableArray<(LongRange Range, long ClampedOffset, long ClampedCount)> ClampableRanges
        = new (LongRange, long, long)[]
        {
            (^5..5, 0, 4), // Contains entire test range
            (^5..1, 0, 1), (^5..^3, 0, 1), // Contains first endpoint
            (^8..^4, 0, 0), (^8..0, 0, 0), // Ends at start
            (^8..^6, 0, 0), // Ends before start
            (6..8, 4, 0), // Starts after end
        }.ToImmutableArray();

    /// <summary>
    /// Ranges for which the offset and length cannot be obtained for a collection of length
    /// <see cref="TestCollectionLength"/> since the length would be negative.
    /// </summary>
    private static readonly ImmutableArray<LongRange> DegenerateRanges
        = new LongRange[] { 4..3, ^3..^4 }.ToImmutableArray();

    /// <summary>
    /// Tests the <see cref="LongRange.IsDegenerate"/> property.
    /// </summary>
    [TestMethod]
    public void TestIsDegenerate()
    {
        foreach (var (range, _, _) in ValidRanges.Concat(ValidOffsetClampableLengthRanges).Concat(ClampableRanges))
        {
            Assert.IsFalse(range.IsDegenerate, $"Range {range} was degenerate.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.IsTrue(range.IsDegenerate, $"Range {range} was not degenerate.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.HasFixedCount()"/> and <see cref="LongRange.HasFixedCount(out long)"/> methods.
    /// </summary>
    [TestMethod]
    public void TestHasFixedCount()
    {
        foreach (var range in new LongRange[] { 1..3, ^3..^1 }) // All have a fixed count of 2
        {
            Assert.IsTrue(range.HasFixedCount(), $"Range {range} did not have fixed count.");
            Assert.IsTrue(range.HasFixedCount(out var fixedCount), $"Range {range} did not have fixed count.");
            Assert.AreEqual(2L, fixedCount, $"Range {range} did not have the expected fixed count.");
        }

        foreach (var range in new LongRange[] { 1..^1, ^1..1 })
        {
            Assert.IsFalse(range.HasFixedCount(), $"Range {range} had fixed count.");
            Assert.IsFalse(range.HasFixedCount(out var fixedCount),
                           $"Range {range} had a fixed count of {fixedCount}.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetOffset(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetOffset()
    {
        foreach (var (range, offset, _) in ValidRanges.Concat(ValidOffsetClampableLengthRanges))
        {
            Assert.AreEqual(offset, range.GetOffset(TestCollectionLength),
                            $"Range {range} offset was not expected value.");
        }

        foreach (var (range, _, _) in ClampableRanges)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => range.GetOffset(TestCollectionLength),
                                                                $"Range {range} offset succeeded.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(() => range.GetOffset(TestCollectionLength),
                                                             $"Range {range} offset succeeded.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetLength(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetLength()
    {
        foreach (var (range, _, length) in ValidRanges)
        {
            Assert.AreEqual(length, range.GetLength(TestCollectionLength),
                            $"Range {range} length was not expected value.");
        }

        foreach (var (range, _, _) in ValidOffsetClampableLengthRanges.Concat(ClampableRanges))
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => range.GetLength(TestCollectionLength),
                                                                $"Range {range} length succeeded.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(() => range.GetLength(TestCollectionLength),
                                                             $"Range {range} length succeeded.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetOffsetAndLength(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetOffsetAndLength()
    {
        foreach (var (range, offset, length) in ValidRanges)
        {
            Assert.AreEqual((offset, length), range.GetOffsetAndLength(TestCollectionLength),
                            $"Range {range} offset and length were not expected values.");
        }

        foreach (var (range, _, _) in ValidOffsetClampableLengthRanges.Concat(ClampableRanges))
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => range.GetOffsetAndLength(TestCollectionLength),
                                                                $"Range {range} offset and length succeeded.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(() => range.GetOffsetAndLength(TestCollectionLength),
                                                             $"Range {range} offset and length succeeded.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetClampedOffset(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetClampedOffset()
    {
        foreach (var (range, offset, _) in ValidRanges
                                            .Concat(ValidOffsetClampableLengthRanges)
                                            .Concat(ClampableRanges))
        {
            Assert.AreEqual(offset, range.GetClampedOffset(TestCollectionLength),
                            $"Range {range} clamped offset was not expected value.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(() => range.GetOffset(TestCollectionLength),
                                                             $"Range {range} offset succeeded.");
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetClampedLength(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetClampedLength()
    {
        foreach (var (range, _, length) in ValidRanges
                                            .Concat(ValidOffsetClampableLengthRanges)
                                            .Concat(ClampableRanges))
        {
            Assert.AreEqual(length, range.GetClampedLength(TestCollectionLength),
                            $"Range {range} clamped length was not expected value.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(() => range.GetClampedLength(TestCollectionLength));
        }
    }

    /// <summary>
    /// Tests the <see cref="LongRange.GetClampedOffsetAndLength(long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetClampedOffsetAndLength()
    {
        foreach (var (range, offset, length) in ValidRanges
                                                    .Concat(ValidOffsetClampableLengthRanges)
                                                    .Concat(ClampableRanges))
        {
            Assert.AreEqual((offset, length), range.GetClampedOffsetAndLength(TestCollectionLength),
                            $"Range {range} offset and length was not expected value.");
        }

        foreach (var range in DegenerateRanges)
        {
            Assert.ThrowsException<DegenerateRangeException>(
                () => range.GetClampedOffsetAndLength(TestCollectionLength),
                $"Range {range} offset and length succeeded.");
        }
    }

    /// <summary>
    /// Tests that the behavior of <see cref="Range.GetOffsetAndLength(int)"/> matches the behavior
    /// of <see cref="LongRange.GetOffsetAndLength(long)"/>.
    /// </summary>
    [TestMethod]
    public void TestGetOffsetAndRangeComparison()
    {
        foreach (var(range, offset, length) in ValidRanges)
        {
            Assert.AreEqual(((int)offset, (int)length), ((Range)range).GetOffsetAndLength(TestCollectionLength),
                            $"Range {range} offset and length did not match System.Range offset and length.");
        }
    }
}
