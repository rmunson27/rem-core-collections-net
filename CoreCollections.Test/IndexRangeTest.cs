using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests of the <see cref="IndexRange"/> class functionality.
/// </summary>
[TestClass]
public class IndexRangeTest
{
    private const int CollectionLength = 4;

    #region Range Test Cases
    private static readonly ImmutableArray<(Range Range, int Offset, int Length)> ValidRangeTests
        = new[]
        {
            #region Both From Start
            ..CollectionLength, // Start to end
            ..(CollectionLength - 1), // First is start
            1..(CollectionLength - 1), // Second is end
            1..CollectionLength, // Strictly in the middle
            1..1, // Empty
            CollectionLength..CollectionLength, // Would be too large, but is empty
            #endregion

            #region First From End
            ^CollectionLength..CollectionLength, // Start to end
            ^CollectionLength..(CollectionLength - 1), // First is start
            ^(CollectionLength - 1)..CollectionLength, // Second is end
            ^(CollectionLength - 1)..(CollectionLength - 1), // Strictly in the middle
            ^(CollectionLength - 1)..1, // Empty
            ^0..CollectionLength, // Would be too large, but is empty
            #endregion

            #region Second From End
            0.., // Start to end
            0..^1, // First is start
            1.., // Second is end
            1..^1, // Strictly in the middle
            (CollectionLength - 1)..^1, // Empty
            CollectionLength..^0, // Would be too large, but is empty
            #endregion

            #region Both From End
            ^CollectionLength.., // Start to end
            ^CollectionLength..^1, // First is start
            ^(CollectionLength - 1).., // Second is end
            ^(CollectionLength - 1)..^1, // Strictly in the middle
            ^1..^1, // Empty
            ^0..^0, // Would be too large, but is empty
            #endregion
        }.Select(r => { var (offset, length) = r.GetOffsetAndLength(CollectionLength); return (r, offset, length); })
        .ToImmutableArray();

    private static readonly ImmutableArray<Range> InvalidRangeTests = new[]
    {
        #region Both From Start
        1..0, // Negative length
        (CollectionLength - 1)..(CollectionLength + 1), // End too large
        CollectionLength..(CollectionLength + 1), // Both too large
        #endregion

        #region First From End
        ^1..1, // Negative length,
        ^1..(CollectionLength + 1), // End too large
        ^0..(CollectionLength + 1), // Both too large
        ^(CollectionLength + 1)..1, // Start too small
        #endregion

        #region Second From End
        (CollectionLength - 1)..^2, // Negative length
        #endregion

        #region Both From End
        ^1..^2, // Negative length
        ^(CollectionLength + 1)..^(CollectionLength - 1), // Start too small
        ^(CollectionLength + 2)..^(CollectionLength + 1) // Both too small
        #endregion
    }.ToImmutableArray();
    #endregion

    /// <summary>
    /// Tests the <see cref="IndexRange.GetOffset(Range, long)"/>, <see cref="IndexRange.GetLength(Range, long)"/>
    /// and <see cref="IndexRange.GetOffsetAndLength(Range, long)"/> methods.
    /// </summary>
    [TestMethod]
    public void TestRangeOffsetAndLength()
    {
        string netImplementationName = $"{nameof(Range)}.{nameof(Range.GetOffsetAndLength)}",
               libImplementationName = $"{nameof(IndexRange)}.{nameof(IndexRange.GetOffsetAndLength)}";
        foreach (var (range, offset, length) in ValidRangeTests)
        {
            Assert.AreEqual((offset, length), range.GetOffsetAndLength((long)CollectionLength),
                            $"Offset and length unequal for range {range}");
            Assert.AreEqual(offset, range.GetOffset(CollectionLength), $"Offset unequal for range {range}");
            Assert.AreEqual(length, range.GetLength(CollectionLength), $"Length unequal for range {range}");
        }

        foreach (var range in InvalidRangeTests)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => range.GetOffsetAndLength(CollectionLength),
                $"{netImplementationName} did not fail for range {range}");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => range.GetOffsetAndLength((long)CollectionLength),
                $"{libImplementationName} did not fail for range {range}");
        }
    }

    /// <summary>
    /// Tests the <see cref="IndexRange.GetOffset(Index, long)"/> method.
    /// </summary>
    [TestMethod]
    public void TestIndexOffset()
    {
        foreach (Index i in new[]
                 {
                    0, ^CollectionLength, // First element
                    1, ^(CollectionLength - 1), // Second element
                    CollectionLength - 2, ^2, // Penultimate element
                    CollectionLength - 1, ^1 // Last element
                 })
        {
            Assert.AreEqual(i.GetOffset(CollectionLength), i.GetOffset((long)CollectionLength),
                            $"Offset unequal for index {i}");
        }
    }
}
