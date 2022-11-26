using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.ObjectModel;

/// <summary>
/// A readonly wrapper for <see cref="ICollection{T}"/> instances that implements the
/// <see cref="IReadOnlyContainer{T}"/> interface.
/// </summary>
public class ReadOnlyContainer<T> : ICollection<T>, IReadOnlyContainer<T>
{
    /// <summary>
    /// The collection wrapped in this instance.
    /// </summary>
    private readonly ICollection<T> _collection;

    bool ICollection<T>.IsReadOnly => true;

    /// <inheritdoc/>
    public int Count => _collection.Count;

    /// <summary>
    /// Constructs a new instance of the <see cref="ReadOnlyContainer{T}"/> class wrapping the collection
    /// passed in.
    /// </summary>
    /// <param name="collection"></param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> was <see langword="null"/>.</exception>
    public ReadOnlyContainer(ICollection<T> collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        _collection = collection;
    }

    [DoesNotReturn]
    void ICollection<T>.Add(T item)
    {
        throw CollectionHelpers.ReadOnlyMutationAttempted;
    }

    [DoesNotReturn]
    void ICollection<T>.Clear()
    {
        throw CollectionHelpers.ReadOnlyMutationAttempted;
    }

    /// <inheritdoc/>
    public bool Contains(T item) => _collection.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _collection.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    [DoesNotReturn]
    bool ICollection<T>.Remove(T item)
    {
        throw CollectionHelpers.ReadOnlyMutationAttempted;
    }

    IEnumerator IEnumerable.GetEnumerator() => (_collection as IEnumerable).GetEnumerator();
}
