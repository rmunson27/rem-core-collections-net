using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

#region Ranges
/// <summary>
/// Represents a range of indices in a collection.
/// </summary>
/// <param name="Start">The inclusive start index of the range.</param>
/// <param name="End">The exclusive end index of the range.</param>
public readonly record struct LongRange(in LongIndex Start, in LongIndex End)
{
    #region Constants
    /// <summary>
    /// A <see cref="LongRange"/> that spans the entire collection, from the start to the end.
    /// </summary>
    public static readonly LongRange All = new(in LongIndex.Start, in LongIndex.End);
    #endregion

    #region Properties
    /// <summary>
    /// Determines if this instance is a degenerate case.
    /// </summary>
    /// <remarks>
    /// A <see cref="LongRange"/> is considered degenerate if the count will be negative regardless of the length of
    /// any collection being indexed because the start index is always strictly less than the end index.
    /// <para/>
    /// Degenerate cases are allowed to more closely match the <see cref="int"/> range type included in .NET core
    /// libraries, as those types allow such values despite the fact that they cannot be typically used to access the
    /// elements of a collection.
    /// </remarks>
    public bool IsDegenerate
    {
        get
        {
            // There are some collections where the length would not be negative if one endpoint is from the start
            // and the other is from the end
            if (Start.IsFromEnd != End.IsFromEnd) return false;

            return Start.IsFromEnd ? Start.Value < End.Value : End.Value < Start.Value;
        }
    }
    #endregion

    #region Factories
    /// <summary>
    /// Creates a new <see cref="LongRange"/> that starts at the specified <see cref="LongIndex"/> and ends at the end
    /// of the collection.
    /// </summary>
    /// <param name="Start">The inclusive index the range should start at.</param>
    /// <returns>
    /// A <see cref="LongRange"/> that starts at <paramref name="Start"/> and ends at the end of the collection.
    /// </returns>
    public static LongRange StartAt(LongIndex Start) => new(Start, LongIndex.End);

    /// <summary>
    /// Creates a new <see cref="LongRange"/> that starts at the start of the collection and ends at the
    /// specified index.
    /// </summary>
    /// <param name="End">The non-inclusive index the range should end at.</param>
    /// <returns>
    /// A <see cref="LongRange"/> that starts at the start of the collection and ends at <paramref name="End"/>.
    /// </returns>
    public static LongRange EndAt(LongIndex End) => new(LongIndex.Start, End);

    /// <summary>
    /// Constructs a new <see cref="LongRange"/> with the specified start index and fixed count.
    /// </summary>
    /// <remarks>
    /// This factory method cannot be used to create degenerate instances, and will therefore throw an
    /// <see cref="ArgumentOutOfRangeException"/> if <paramref name="Count"/> is negative.
    /// </remarks>
    /// <param name="Start">The inclusive start index of the range.</param>
    /// <param name="Count">The number of elements in the range.</param>
    /// <returns>
    /// A new <see cref="LongRange"/> starting at <paramref name="Start"/> and containing <paramref name="Count"/> indices.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Count"/> was negative.</exception>
    public static LongRange FromStartAndCount(LongIndex Start, long Count)
    {
        if (Count < 0) throw new ArgumentOutOfRangeException(nameof(Count), Count, "Count cannot be negative.");
        return new(in Start, Start.ShiftBy(Count));
    }
    #endregion

    #region Offset And Count Computing
    /// <summary>
    /// Determines whether or not this instance has a fixed count regardless of the length of the collection being
    /// indexed, provided the collection is long enough to contain the range.
    /// </summary>
    /// <remarks>
    /// This method will return <see langword="true"/> for degenerate instances, as they do have a calculable fixed
    /// count - it is just negative, which is not applicable to most non-exceptional cases.
    /// </remarks>
    /// <returns>
    /// Whether or not the collection-independent count can be computed.
    /// If it returns, this method will return <see langword="true"/> if and only if both <see cref="Start"/> and
    /// <see cref="End"/> are from the same side of the collection.
    /// </returns>
    public bool HasFixedCount() => Start.IsFromEnd == End.IsFromEnd;

    /// <summary>
    /// Determines whether or not this instance has a fixed count regardless of the length of the collection being
    /// indexed, provided the collection is long enough to contain the range.
    /// If so, the fixed count will be returned in an <see langword="out"/> parameter.
    /// </summary>
    /// <remarks>
    /// This method will return <see langword="true"/> for degenerate instances, as they do have a calculable fixed
    /// count - it is just negative, which is not applicable to most non-exceptional cases.
    /// </remarks>
    /// <param name="fixedCount">
    /// An <see langword="out"/> parameter to set to the collection-independent count, if it can be computed.
    /// <para/>
    /// If this instance is degenerate, this will be a negative number.
    /// </param>
    /// <returns>
    /// Whether or not the collection-independent count can be computed.
    /// If it returns, this method will return <see langword="true"/> if and only if both <see cref="Start"/> and
    /// <see cref="End"/> are from the same side of the collection.
    /// </returns>
    public bool HasFixedCount(out long fixedCount)
    {
        long rawCount;
        if (Start.IsFromEnd && End.IsFromEnd) rawCount = Start.Value - End.Value;
        else if (Start.IsFromStart && End.IsFromStart) rawCount = End.Value - Start.Value;
        else return Try.Failure(out fixedCount); // This cannot be a degenerate case

        return Try.Success(out fixedCount, rawCount);
    }

    /// <summary>
    /// Computes the offset of this instance with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="collectionLength">The length of the collection to compute the offset into.</param>
    /// <returns>
    /// The offset of this instance with respect to a collection of length <paramref name="collectionLength"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or the offset would be negative or overflow beyond the
    /// largest valid index of the collection.
    /// </exception>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless of the length of any collection
    /// being indexed.
    /// </exception>
    public long GetOffset(long collectionLength)
    {
        this.ThrowIfDegenerate();

        var rawOffset = EnsureOffsetNonNegative(GetOffsetUnchecked(collectionLength.ThrowIfCollectionLengthNegative()),
                                                collectionLength);
        if (rawOffset > collectionLength)
        {
            throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                  "Offset would be out of range of the collection.");
        }

        return rawOffset;
    }

    /// <summary>
    /// Computes the offset of this instance with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// 
    /// If this instance would overflow the collection, it will be treated as being clamped to the bounds of
    /// the collection.
    /// </summary>
    /// <param name="collectionLength">The length of the collection to compute the offset into.</param>
    /// <returns>
    /// The offset of this instance with respect to a collection of length <paramref name="collectionLength"/>,
    /// clamped to the bounds of the collection if necessary.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collectionLength"/> was negative.</exception>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless of the length of any collection
    /// being indexed.
    /// </exception>
    public long GetClampedOffset(long collectionLength)
        => GetOffsetUnchecked(collectionLength.ThrowIfCollectionLengthNegative())
            .ClampToValidIndices(collectionLength, allowCollectionLength: true);

    /// <summary>
    /// Gets the length of this instance with respect to a collection of length <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="collectionLength">The length of the collection to compute the length with respect to.</param>
    /// <returns>
    /// The length of this instance with respect to a collection of length <paramref name="collectionLength"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or this instance would overflow beyond the bounds of a
    /// collection of length <paramref name="collectionLength"/>.
    /// </exception>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless
    /// of <paramref name="collectionLength"/>.
    /// </exception>
    public long GetLength(long collectionLength)
    {
        this.ThrowIfDegenerate();
        collectionLength.ThrowIfCollectionLengthNegative();

        var offset = EnsureOffsetNonNegative(Start.GetOffsetUnchecked(collectionLength), collectionLength);
        if (offset > collectionLength)
        {
            throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                  "Offset would be out of range of the collection.");
        }
        var length = EnsureLengthNonNegative(GetLengthFromOffsetUnchecked(collectionLength, offset),
                                             collectionLength);

        EnsureDoesNotOverflow(offset, length, collectionLength);

        return length;
    }

    /// <summary>
    /// Gets the length of this instance with respect to a collection of length <paramref name="collectionLength"/>.
    /// If this instance would overflow the collection, it will be treated as being clamped to the bounds of
    /// the collection.
    /// </summary>
    /// <param name="collectionLength">
    /// The length of the collection to compute the length with respect to.
    /// </param>
    /// <returns>
    /// The length of this instance with respect to a collection of length <paramref name="collectionLength"/>,
    /// clamped to the bounds of the collection if necessary.
    /// </returns>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless
    /// of <paramref name="collectionLength"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collectionLength"/> was negative.</exception>
    public long GetClampedLength(long collectionLength) => GetClampedLengthWithClampedOffset(collectionLength, out _);

    /// <summary>
    /// Gets the offset and length of this instance with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="collectionLength">
    /// The length of the collection to compute the offset and length with respect to.
    /// </param>
    /// <returns>
    /// A tuple containing the offset and length of this instance with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// </returns>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless
    /// of <paramref name="collectionLength"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative, or this instance would overflow beyond the bounds of a
    /// collection of length <paramref name="collectionLength"/>.
    /// </exception>
    public (long Offset, long Length) GetOffsetAndLength(long collectionLength)
    {
        this.ThrowIfDegenerate();
        collectionLength.ThrowIfCollectionLengthNegative();

        long offset = EnsureLengthNonNegative(Start.GetOffsetUnchecked(collectionLength), collectionLength),
             length = EnsureLengthNonNegative(GetLengthFromOffsetUnchecked(collectionLength, offset),
                                              collectionLength);

        EnsureDoesNotOverflow(offset, length, collectionLength);

        return (offset, length);
    }

    /// <summary>
    /// Gets the offset and length of this instance with respect to a collection of
    /// length <paramref name="collectionLength"/>.
    /// 
    /// If this instance would overflow the collection, it will be treated as being clamped to the bounds of
    /// the collection.
    /// </summary>
    /// <param name="collectionLength">
    /// The length of the collection to compute the offset and length with respect to.
    /// </param>
    /// <returns>
    /// A tuple containing the offset and length of this instance with respect to a collection of length
    /// <paramref name="collectionLength"/>, clamped to the bounds of the collection if necessary.
    /// </returns>
    /// <exception cref="DegenerateRangeException">
    /// This instance is degenerate, i.e. its count will always be negative regardless
    /// of <paramref name="collectionLength"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collectionLength"/> was negative.</exception>
    public (long Offset, long Length) GetClampedOffsetAndLength(long collectionLength)
    {
        var length = GetClampedLengthWithClampedOffset(collectionLength, out var offset);
        return (offset, length);
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another <see cref="LongRange"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(LongRange other) => Start == other.Start && End == other.End;

    /// <summary>
    /// Gets a hash code representing this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Start, End);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents this instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"[{Start.FormattedValue}..{End.FormattedValue}]";
    #endregion

    #region Conversions
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    /// <summary>
    /// Implicitly converts a <see cref="Range"/> to a <see cref="LongRange"/>.
    /// </summary>
    /// <param name="range"></param>
    public static implicit operator LongRange(Range range) => new(range.Start, range.End);

    /// <summary>
    /// Explicitly converts a <see cref="LongRange"/> to a <see cref="Range"/>.
    /// </summary>
    /// <param name="range"></param>
    /// <exception cref="OverflowException">
    /// One or both of the start and end of the <see cref="LongRange"/> was too large to fit in a
    /// <see cref="LongIndex"/>, so the cast could not be completed.
    /// </exception>
    public static explicit operator Range(LongRange range) => new((Index)range.Start, (Index)range.End);
#endif
    #endregion

    #region Helpers
    /// <summary>
    /// Gets the clamped offset and length.  If the range length is negative, 0 will be returned.
    /// </summary>
    /// <param name="collectionLength"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collectionLength"/> was negative.</exception>
    /// <exception cref="DegenerateRangeException">This instance is degenerate.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long GetClampedLengthWithClampedOffset(long collectionLength, out long offset)
    {
        if (IsDegenerate) // Treat as starting at the start and having length 0
        {

            offset = GetOffsetUnchecked(collectionLength)
                        .ClampToValidIndices(collectionLength, allowCollectionLength: true);
            return 0;
        }

        collectionLength.ThrowIfCollectionLengthNegative();

        offset = Start.GetOffsetUnchecked(collectionLength);
        if (offset > collectionLength)
        {
            offset = collectionLength;
            return 0;
        }

        var length = Math.Max(0, GetLengthFromOffsetUnchecked(collectionLength, offset));

        // Clamp the offset to determine the clamped length
        // If the offset is less than zero, the length will have to be adjusted accordingly before clamping
        if (offset < 0)
        {
            length = (length + offset).Clamp(0, collectionLength);
            offset = 0;
            return length;
        }
        else
        {
            offset = Math.Min(collectionLength, offset);
            return Math.Min(collectionLength - offset, length);
        }
    }

    #region Unchecked
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long GetLengthFromOffsetUnchecked(long collectionLength, long offset)
        // No need to add 1 since ranges are endpoint-exclusive
        => End.GetOffsetUnchecked(collectionLength) - offset;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long GetOffsetUnchecked(long collectionLength) => Start.GetOffsetUnchecked(collectionLength);
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
    private static long EnsureLengthNonNegative(long length, long collectionLength)
        => EnsureNonNegative(length, collectionLength, "Range length would be negative for collection.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureOffsetNonNegative(long offset, long collectionLength)
        => EnsureNonNegative(offset, collectionLength, "Range offset would be negative for collection.");

    /// <summary>
    /// Ensures the supplied <see cref="long"/> value is not negative, throwing an exception indicating that the
    /// specified collection length is out of range if so (as <paramref name="collectionLength"/> is the argument
    /// to the offset or length getter this method is called from).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="collectionLength"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long EnsureNonNegative(long value, long collectionLength, string message)
        => value < 0
            ? throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength, message)
            : value;
    #endregion
    #endregion
}

/// <summary>
/// An exception thrown when a range is degenerate (i.e. its start point is always strictly after its end point).
/// </summary>
public class DegenerateRangeException : InvalidOperationException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="DegenerateRangeException"/> with a default error message.
    /// </summary>
    public DegenerateRangeException()
    : base("Range is degenerate - its start point is always strictly after its end point.")
    { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DegenerateRangeException"/> with the specified error message.
    /// </summary>
    /// <param name="message"></param>
    public DegenerateRangeException(string message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DegenerateRangeException"/> with the specified error message and
    /// inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public DegenerateRangeException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="DegenerateRangeException"/> from the serialization data passed in
    /// (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected DegenerateRangeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Extension methods for the <see cref="LongRange"/> struct.
/// </summary>
public static class LongRangeExtensions
{
    /// <summary>
    /// Throws a <see cref="DegenerateRangeException"/> if the current <see cref="LongRange"/> is degenerate, i.e. its
    /// start index is always strictly after its end index.
    /// </summary>
    /// <param name="range"></param>
    /// <returns>The current <see cref="LongRange"/>.</returns>
    /// <exception cref="DegenerateRangeException">The current <see cref="LongRange"/> is degenerate.</exception>
    public static ref readonly LongRange ThrowIfDegenerate(in this LongRange range)
    {
        if (range.IsDegenerate) throw new DegenerateRangeException();
        else return ref range;
    }
}
#endregion

/// <summary>
/// A type that can be used to index a collection either from the beginning or the end.
/// </summary>
public readonly record struct LongIndex
{
    /// <summary>
    /// A <see cref="LongIndex"/> that points beyond the last element in a collection.
    /// </summary>
    public static readonly LongIndex End = new(~0L);

    /// <summary>
    /// A <see cref="LongIndex"/> that points to the first element in a collection.
    /// </summary>
    public static readonly LongIndex Start = new(0L);

    /// <summary>
    /// Indicates whether or not the value is from the start of a collection.
    /// </summary>
    public bool IsFromStart => _value >= 0;

    /// <summary>
    /// Indicates whether or not the value is from the end of the collection.
    /// </summary>
    public bool IsFromEnd => _value < 0;

    /// <summary>
    /// Gets the value of this <see cref="LongIndex"/>.
    /// </summary>
    public long Value => _value < 0 ? ~_value : _value;
    internal readonly long _value;

    /// <summary>
    /// Creates a new <see cref="LongIndex"/> from the end of a collection at the specified index position.
    /// </summary>
    /// <param name="Value">The value of the index to create.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Value"/> was negative.</exception>
    public static LongIndex FromEnd(long Value) => new(Value, IsFromEnd: true);

    /// <summary>
    /// Creates a new <see cref="LongIndex"/> from the start of a collection at the specified index position.
    /// </summary>
    /// <param name="Value">The value of the index to create.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Value"/> was negative.</exception>
    public static LongIndex FromStart(long Value) => new(Value, IsFromEnd: false);

    /// <summary>
    /// Constructs a new instance of the <see cref="LongIndex"/> struct from the index value and whether or not the
    /// instance is from the end.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="IsFromEnd"></param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Value"/> was negative.</exception>
    public LongIndex(long Value, bool IsFromEnd)
    {
        if (Value < 0) throw new ArgumentOutOfRangeException(nameof(Value), Value, "Value cannot be negative.");
        _value = IsFromEnd ? ~Value : Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LongIndex(long Value)
    {
        _value = Value;
    }

    /// <summary>
    /// Calculates the offset of this instance from the start of a collection of
    /// length <paramref name="collectionLength"/>.
    /// If the index is outside of the range of valid indices for the collection, it will be treated as being clamped
    /// to the bounds of the collection, rather than throwing an exception.
    /// </summary>
    /// <param name="collectionLength">The collection length to use to compute the offset.</param>
    /// <param name="allowCollectionLength">
    /// Whether or not allowing the index to be the length of the collection is to be considered valid.
    /// Setting this to <see langword="true"/> may be necessary in order to use the length of the collection as an
    /// indicator that the entire collection has been traversed.
    /// </param>
    /// <returns>
    /// The offset of this instance from the start of a collection of length <paramref name="collectionLength"/>,
    /// clamped to the bounds of the collection if necessary.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collectionLength"/> was negative.</exception>
    public long GetClampedOffset(long collectionLength, bool allowCollectionLength = false)
        => GetOffsetUnchecked(collectionLength.ThrowIfCollectionLengthNegative())
            .ClampToValidIndices(collectionLength, allowCollectionLength);

    /// <summary>
    /// Calculates the offset from the start of a collection with the specified collection length.
    /// </summary>
    /// <param name="collectionLength">The collection length to use to compute the offset.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="collectionLength"/> was negative
    /// <para/>
    /// OR
    /// <para/>
    /// The index would be negative for a collection of length <paramref name="collectionLength"/>
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="allowCollectionLength"/> is <see langword="true"/> and the index would be out of range for
    /// the collection
    /// <para/>
    /// <paramref name="allowCollectionLength"/> is <see langword="false"/> and the index would be out of the range
    /// of valid indices for the collection.
    /// </exception>
    public long GetOffset(long collectionLength, bool allowCollectionLength = false)
    {
        var offset = GetOffsetUnchecked(collectionLength.ThrowIfCollectionLengthNegative());
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                              $"Index would be negative for the collection.");
        if (allowCollectionLength)
        {
            if (offset > collectionLength)
            {
                throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                      "Index would be out of range for the collection.");
            }
        }
        else
        {
            if (offset >= collectionLength)
            {
                throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                      "Index would be out of range of valid indices" +
                                                        " for the collection.");
            }
        }

        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LongIndex ShiftBy(long amount)
    {
        if (IsFromEnd)
        {
            var newValue = _value - amount;
            if (newValue >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Value), ~newValue, "Value cannot be negative.");
            }
            return new(newValue);
        }
        else
        {
            var newValue = _value + amount;
            if (newValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Value), newValue, "Value cannot be negative.");
            }
            return new(newValue);
        }
    }

    /// <summary>
    /// Provides the logic of getting an offset for the index without checking for exceptional cases.
    /// </summary>
    /// <remarks>
    /// In particular, this method does not check that the resulting offset is non-negative.
    /// </remarks>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal long GetOffsetUnchecked(long collectionLength) => _value < 0 ? collectionLength + _value + 1 : _value;

    /// <summary>
    /// Determines whether this <see cref="LongIndex"/> is equal to another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(LongIndex other) => _value == other._value;

    /// <summary>
    /// Gets a hash code representing this <see cref="LongIndex"/>.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _value.GetHashCode();

    /// <summary>
    /// Gets a string representing this <see cref="LongIndex"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"[{FormattedValue}]";

    /// <summary>
    /// Gets a formatted string representing the value of this <see cref="LongIndex"/>.
    /// </summary>
    internal string FormattedValue => IsFromEnd ? $"^{~_value}" : _value.ToString();

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    /// <summary>
    /// Implicitly converts an <see cref="Index"/> to a <see cref="LongIndex"/>.
    /// </summary>
    /// <param name="index"></param>
    public static implicit operator LongIndex(Index index) => new(index.Value, IsFromEnd: index.IsFromEnd);

    /// <summary>
    /// Explicitly converts a <see cref="LongIndex"/> to an <see cref="Index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="OverflowException">
    /// The offset was too large to fit in an <see cref="int"/>, so the cast could not be completed.
    /// </exception>
    public static explicit operator Index(LongIndex index) => new((int)index.Value, fromEnd: index.IsFromEnd);
#endif

    /// <summary>
    /// Implicitly converts a <see cref="long"/> to a <see cref="LongIndex"/>.
    /// </summary>
    /// <param name="l"></param>
    public static implicit operator LongIndex(long l) => new(l);
}

/// <summary>
/// Extension methods used in functionality for this file.
/// </summary>
file static class Extensions
{
    /// <summary>
    /// Clamps the current <see cref="long"/> to the range of valid indices for a collection of the specified length.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="collectionLength"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ClampToValidIndices(this long value, long collectionLength, bool allowCollectionLength = false)
        => value.Clamp(0, allowCollectionLength ? collectionLength : collectionLength - 1);

    /// <summary>
    /// Clamps the current <see cref="long"/> to a range specified by minimum and maximum values.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Clamp(this long value, long min, long max) => Math.Min(Math.Max(value, min), max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ThrowIfCollectionLengthNegative(this long collectionLength)
        => collectionLength < 0
            ? throw new ArgumentOutOfRangeException(nameof(collectionLength), collectionLength,
                                                    "Collection length cannot be negative.")
            : collectionLength;
}
