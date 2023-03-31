using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
/// <summary>
/// Extension methods and static functionality for working with the <see cref="Index"/> and
/// <see cref="Range"/> classes.
/// </summary>
public static class IndexRange
{
    /// <summary>
    /// Gets the offset of the current <see cref="Range"/> with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or out of range for the current <see cref="Range"/>.
    /// </exception>
    public static long GetOffset(this Range range, long collectionLength)
        => range.GetOffsetUnchecked(collectionLength.EnsureCollectionLengthNonNegative())
                .EnsureRangeOffsetNonNegative(collectionLength);

    /// <summary>
    /// Gets the length of the current <see cref="Range"/> with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or out of range for the current <see cref="Range"/>.
    /// </exception>
    public static long GetLength(this Range range, long collectionLength)
    {
        collectionLength.EnsureCollectionLengthNonNegative();

        long rangeOffset = range.Start.GetOffsetUnchecked(collectionLength)
                                      .EnsureRangeOffsetNonNegative(collectionLength),
             rangeLength = range.GetLengthFromOffsetUnchecked(collectionLength, rangeOffset)
                                .EnsureRangeLengthNonNegative(collectionLength);

        EnsureDoesNotOverflow(rangeOffset, rangeLength, collectionLength);

        return rangeLength;
    }

    /// <summary>
    /// Gets the offset and length the current <see cref="Range"/> represents, given the collection length passed in.
    /// </summary>
    /// <param name="range"></param>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or out of range for the current <see cref="Range"/>.
    /// </exception>
    public static (long Offset, long Length) GetOffsetAndLength(this Range range, long collectionLength)
    {
        collectionLength.EnsureCollectionLengthNonNegative();

        long rangeOffset = range.Start.GetOffsetUnchecked(collectionLength)
                                      .EnsureRangeOffsetNonNegative(collectionLength),
             rangeLength = range.GetLengthFromOffsetUnchecked(collectionLength, rangeOffset)
                                .EnsureRangeLengthNonNegative(collectionLength);

        EnsureDoesNotOverflow(rangeOffset, rangeLength, collectionLength);

        return (Offset: rangeOffset, Length: rangeLength);
    }

    /// <summary>
    /// Gets the offset the current <see cref="Index"/> represents, given the collection length passed in.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or out of range for the current <see cref="Index"/>.
    /// </exception>
    public static long GetOffset(this Index index, long collectionLength)
        => index.GetOffsetUnchecked(collectionLength.EnsureCollectionLengthNonNegative())
                .EnsureIndexOffsetNonNegative(collectionLength);

    #region Helpers
    #region Unchecked
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GetLengthFromOffsetUnchecked(this Range range, long collectionLength, long offset)
        // No need to add 1 since ranges are endpoint-exclusive
        => range.End.GetOffsetUnchecked(collectionLength) - offset;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GetOffsetUnchecked(this Range range, long collectionLength)
        => range.Start.GetOffsetUnchecked(collectionLength);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GetOffsetUnchecked(this Index index, long collectionLength)
        => index.IsFromEnd ? collectionLength - index.Value : index.Value;
    #endregion

    #region Checks
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EnsureDoesNotOverflow(long offset, long length, long collectionLength)
    {
        if (offset + length > collectionLength)
        {
            throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                  "Range would extend beyond the end of the collection.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureIndexOffsetNonNegative(this long offset, long collectionLength)
        => offset.EnsureNonNegative(collectionLength, "Index offset would be negative for collection.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureRangeLengthNonNegative(this long length, long collectionLength)
        => length.EnsureNonNegative(collectionLength, "Range length would be negative for collection.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureRangeOffsetNonNegative(this long offset, long collectionLength)
        => offset.EnsureNonNegative(collectionLength, "Range offset would be negative for collection.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureCollectionLengthNonNegative(this long collectionLength)
        => collectionLength.EnsureNonNegative(collectionLength, "Collection length cannot be negative.");

    /// <summary>
    /// Ensures the current <see cref="long"/> value is not negative, throwing an exception indicating that the
    /// specified collection length is out of range if so (as <paramref name="collectionLength"/> is the argument
    /// to the offset or length getter this method is called from).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="collectionLength"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureNonNegative(this long value, long collectionLength, string message)
        => value < 0
            ? throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength, message)
            : value;
    #endregion
    #endregion
}
#endif
