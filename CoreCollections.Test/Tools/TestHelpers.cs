using Rem.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

#region Concrete
/// <summary>
/// A class representing test method functionality for the <see cref="ReadOnlyArray{T}"/> type.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal sealed class ReadOnlyArrayTestHelpers<TItem> : TestHelpers<ReadOnlyArray<TItem>, TItem>
{
    /// <inheritdoc/>
    public override ReadOnlyArray<TItem> CreateRange(params TItem[] range) => range.AsReadOnlyArray();

    /// <inheritdoc/>
    public override bool SequenceEqual(
        [DisallowDefault] ReadOnlyArray<TItem> lhs, [DisallowDefault] ReadOnlyArray<TItem> rhs)
        => lhs.SequenceEqual(rhs);
}

/// <summary>
/// A class representing test method functionality for the <see cref="ImmutableArray{T}"/> type.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal sealed class ImmutableArrayTestHelpers<TItem> : TestHelpers<ImmutableArray<TItem>, TItem>
{
    /// <inheritdoc/>
    public override ImmutableArray<TItem> CreateRange(params TItem[] range) => range.ToImmutableArray();

    /// <inheritdoc/>
    public override bool SequenceEqual(
        [DisallowDefault] ImmutableArray<TItem> lhs, [DisallowDefault] ImmutableArray<TItem> rhs)
        => lhs.SequenceEqual(rhs);
}

/// <summary>
/// A class representing test method functionality for the <see cref="IEnumerable{T}"/> type.
/// </summary>
/// <typeparam name="TItem"></typeparam>
internal sealed class EnumerableTestHelpers<TItem> : TestHelpers<IEnumerable<TItem>, TItem>
{
    /// <inheritdoc/>
    public override IEnumerable<TItem> CreateRange(params TItem[] range) => range;

    /// <inheritdoc/>
    public override bool SequenceEqual(IEnumerable<TItem> lhs, IEnumerable<TItem> rhs) => lhs.SequenceEqual(rhs);
}
#endregion

#region Abstract
/// <summary>
/// A class representing test method functionality for the <typeparamref name="TEnumerable"/> generic type.
/// </summary>
/// <typeparam name="TEnumerable">The type of <see cref="IEnumerable{T}"/> under test.</typeparam>
internal abstract class TestHelpers<TEnumerable, TItem> where TEnumerable : notnull, IEnumerable<TItem>
{
    /// <summary>
    /// Creates a <typeparamref name="TEnumerable"/> from a range of integers.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public abstract TEnumerable CreateRange(params TItem[] range);

    /// <summary>
    /// Determines if two <typeparamref name="TEnumerable"/> instances are sequence-equal.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public abstract bool SequenceEqual([DisallowDefault] TEnumerable lhs, [DisallowDefault] TEnumerable rhs);
}
#endregion
