using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Static functionality for working with instances of the <see cref="IReadOnlyContainer{T}"/> interface.
/// </summary>
public static class ReadOnlyContainer
{
    /// <summary>
    /// Gets an <see cref="IReadOnlyContainer{T}"/> wrapper for the <see cref="IImmutableSet{T}"/> implementation
    /// passed in.
    /// </summary>
    /// <remarks>
    /// This method is to allow <see cref="IImmutableSet{T}"/> instances to be converted to
    /// <see cref="IReadOnlyContainer{T}"/> easily.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <returns></returns>
    public static IReadOnlyContainer<T> AsReadOnly<T>(this IImmutableSet<T> set)
        => new ReadOnlyImmutableSetWrapper<T>(set);

    /// <summary>
    /// Used to wrap <see cref="IImmutableSet{T}"/> instances in <see cref="IReadOnlyContainer{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private sealed class ReadOnlyImmutableSetWrapper<T> : IReadOnlyContainer<T>
    {
        /// <summary>
        /// The internally wrapped <see cref="IImmutableSet{T}"/>.
        /// </summary>
        private readonly IImmutableSet<T> _set;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyImmutableSetWrapper(IImmutableSet<T> set) { _set = set; }

        /// <inheritdoc/>
        public int Count => _set.Count;

        /// <inheritdoc/>
        public bool Contains(T value) => _set.Contains(value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
    }
}

/// <summary>
/// Represents a readonly collection for which containment can be determined.
/// </summary>
/// <remarks>
/// This interface is just <see cref="IReadOnlyCollection{T}"/> but with an analog of the
/// <see cref="ICollection{T}.Contains(T)"/> method added.
/// </remarks>
public interface IReadOnlyContainer<T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Determines if the <see cref="IReadOnlyContainer{T}"/> contains the given value.
    /// </summary>
    /// <param name="value">The value to test for.</param>
    /// <returns></returns>
    public bool Contains(T value);
}
