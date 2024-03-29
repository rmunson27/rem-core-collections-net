﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rem.Core.ComponentModel;

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

    /// <summary>
    /// Constructs a new instance of the <see cref="ReadOnlyContainer{T}"/> class wrapping the
    /// <see cref="ReadOnlyArray{T}"/> passed in without boxing it.
    /// </summary>
    /// <param name="collection"></param>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="collection"/> was <see langword="default"/>.
    /// </exception>
    public ReadOnlyContainer(ReadOnlyArray<T> collection)
    {
        if (collection.IsDefault) throw new StructArgumentDefaultException(nameof(collection));
        _collection = collection._array; // Safe to pass the array
    }

    [DoesNotReturn]
    void ICollection<T>.Add(T item)
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    void ICollection<T>.Clear()
    {
        throw Enumerables.MutationNotSupported;
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
        throw Enumerables.MutationNotSupported;
    }

    IEnumerator IEnumerable.GetEnumerator() => (_collection as IEnumerable).GetEnumerator();
}
