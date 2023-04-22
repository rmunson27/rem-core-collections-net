using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Rem.Core.Attributes;
using Rem.Core.Collections.Enumeration;
using Rem.Core.ComponentModel;

namespace Rem.Core.Collections;

/// <summary>
/// Represents a read-only array.
/// </summary>
/// <remarks>
/// This struct creates a thin, read-only wrapper around an array of type <typeparamref name="T"/>.
/// The array <i>can still be modified</i>, just not through the wrapper.
/// <para/>
/// This type returns readonly references to its elements from its indexers, so they are valid return values in
/// <see langword="ref"/> <see langword="readonly"/> contexts.
/// </remarks>
/// <typeparam name="T">The element type of the array.</typeparam>
public readonly struct ReadOnlyArray<T>
    : IDefaultableStruct,
      IList<T>, IReadOnlyContainer<T>, IReadOnlyList<T>, ISpecifiedEnumerable<ArrayEnumerator<T>, T>
{
    #region Constants
    /// <summary>
    /// Gets an empty <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <remarks>
    /// This array wraps the result of a call to <see cref="Array.Empty{T}()"/> to avoid zero-length allocations.
    /// </remarks>
    public static ReadOnlyArray<T> Empty => new(Array.Empty<T>());
    #endregion

    #region Properties And Fields
    /// <inheritdoc/>
    public bool IsDefault => _array is null;

    /// <summary>
    /// Gets the length of the wrapped array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public int Length => GetCount();

    /// <summary>
    /// Gets the length of the wrapped array as a 64-bit integer.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public long LongLength => ThrowIfDefault()._array.LongLength;

    [DoesNotReturnIfInstanceDefault]
    int ICollection<T>.Count => GetCount();

    [DoesNotReturnIfInstanceDefault]
    int IReadOnlyCollection<T>.Count => GetCount();

    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[int index] => ref ThrowIfDefault()._array[index];

    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[long index] => ref ThrowIfDefault()._array[index];

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="this[LongIndex]"/>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[Index index] => ref this[(LongIndex)index];
#endif

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[LongIndex index]
    {
        get
        {
            ThrowIfDefault();
            var offset = index.GetOffset(LongLength);
            return ref _array[offset];
        }
    }

    [DoesNotReturnIfInstanceDefault]
    T IReadOnlyList<T>.this[int index] => GetAtIndex(index);

    [DoesNotReturnIfInstanceDefault]
    T IList<T>.this[int index]
    {
        get => GetAtIndex(index);
        set => throw Enumerables.MutationNotSupported;
    }

    internal readonly T[] _array;

    bool ICollection<T>.IsReadOnly => true;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="ReadOnlyArray{T}"/> struct wrapping the specified array.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// <para/>
    /// Passing <see langword="null"/> as the <paramref name="array"/> parameter will result in the default instance
    /// of this type.
    /// </remarks>
    /// <param name="array">The array to construct a readonly wrapper for.</param>
    [NotDefaultIfNotDefault(nameof(array))]
    public ReadOnlyArray(T[]? array) { _array = array!; }
    #endregion

    #region Methods
    #region IEnumerable
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    [DoesNotReturnIfInstanceDefault]
    public ArrayEnumerator<T> GetEnumerator() => new(ThrowIfDefault()._array);

    [DoesNotReturnIfInstanceDefault]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [DoesNotReturnIfInstanceDefault]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region IList
    #region Public
    /// <summary>
    /// Determines the index of a specified element in the <see cref="ReadOnlyArray{T}"/>.
    /// </summary>
    /// <param name="item">The object to locate in the array.</param>
    /// <returns>
    /// The index of <paramref name="item"/> if found in the array; otherwise -1;
    /// </returns>
    [DoesNotReturnIfInstanceDefault]
    public int IndexOf(T item) => (ThrowIfDefault()._array as IList<T>).IndexOf(item);

    /// <summary>
    /// Copies the elements of the current <see cref="ReadOnlyArray{T}"/> to the specified array starting at the
    /// specified index.
    /// </summary>
    /// <param name="array">
    /// The array that is the destination of elements copied from this <see cref="ReadOnlyArray{T}"/>.
    /// </param>
    /// <inheritdoc/>
    [DoesNotReturnIfInstanceDefault]
    public void CopyTo(T[] array, int arrayIndex) => ThrowIfDefault()._array.CopyTo(array, arrayIndex);
    #endregion

    #region Not Supported
    [DoesNotReturn]
    void IList<T>.Insert(int index, T item)
    {
        throw Enumerables.MutationNotSupported;
    }

    [DoesNotReturn]
    void IList<T>.RemoveAt(int index)
    {
        throw Enumerables.MutationNotSupported;
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

    [DoesNotReturn]
    bool ICollection<T>.Remove(T item)
    {
        throw Enumerables.MutationNotSupported;
    }
    #endregion
    #endregion

    #region LINQ
    #region Chunk
    /// <inheritdoc cref="Chunk(long)"/>
    public IEnumerable<ReadOnlyArraySlice<T>> Chunk(int size) => Chunk((long)size);

    /// <summary>
    /// Separates the current sequence into a series of chunks of maximum size <paramref name="size"/>.
    /// </summary>
    /// <param name="size">The maximum size of the chunks to separate the current sequence into.</param>
    /// <returns>
    /// A sequence of chunks of maximum size <paramref name="size"/>.
    /// All chunks returned except the last will have size exactly <paramref name="size"/>, where the last chunk
    /// may be shorter.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
    public IEnumerable<ReadOnlyArraySlice<T>> Chunk(long size)
        => size < 1
            ? throw new ArgumentOutOfRangeException(nameof(size), size, $"Size must be at least 1.")
            : ChunkUnchecked(size);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<ReadOnlyArraySlice<T>> ChunkUnchecked(long size)
    {
        if (size >= LongLength) yield return (ReadOnlyArraySlice<T>)this;
        else
        {
            long offset = 0;
            while (offset < LongLength)
            {
                var count = Math.Min(LongLength - offset, size);
                yield return new(_array, offset, count);
                offset += count;
            }
        }
    }
    #endregion

    #region Slicing
    /// <inheritdoc cref="Skip(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Skip(int count) => Skip((long)count);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array offset by the given number of elements.
    /// </summary>
    /// <param name="count"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The count was negative or out of range for the array.
    /// </exception>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Skip(long count) => ((ReadOnlyArraySlice<T>)this).Skip(count);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array with the given number of elements removed
    /// from the end.
    /// </summary>
    /// <param name="count"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The offset was negative or out of range for the array.
    /// </exception>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipLast(long count) => ((ReadOnlyArraySlice<T>)this).SkipLast(count);

    /// <inheritdoc cref="SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

    /// <inheritdoc cref="SkipWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, int, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

    /// <summary>
    /// Creates a <see cref="ReadOnlyArraySlice{T}"/> representing the array with elements skipped until the specified
    /// predicate fails.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> SkipWhile(Func<T, long, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).SkipWhile(predicate);

#if INDEX_RANGE_SUPPORTED
    /// <inheritdoc cref="Take(LongRange)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(Range range) => ((ReadOnlyArraySlice<T>)this).Take(range);
#endif

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> representing the given range of elements from the array.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(LongRange range) => ((ReadOnlyArraySlice<T>)this).Take(range);

    /// <inheritdoc cref="Take(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(int count) => Take((long)count);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> representing the given number of elements from the beginning
    /// of the array.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> Take(long count) => ((ReadOnlyArraySlice<T>)this).Take(count);

    /// <inheritdoc cref="TakeWhile(Func{T, long, bool})"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).TakeWhile(predicate);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing elements taken from this array starting at the
    /// beginning and ending when the specified predicate fails.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeWhile(Func<T, long, bool> predicate)
        => ((ReadOnlyArraySlice<T>)this).TakeWhile(predicate);

    /// <inheritdoc cref="TakeLast(long)"/>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(int count) => ((ReadOnlyArraySlice<T>)this).TakeLast(count);

    /// <summary>
    /// Creates a new <see cref="ReadOnlyArraySlice{T}"/> containing the specified number of elements taken from the
    /// end of the array.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    [return: NonDefaultableStruct]
    [DoesNotReturnIfInstanceDefault]
    public ReadOnlyArraySlice<T> TakeLast(long count) => ((ReadOnlyArraySlice<T>)this).TakeLast(count);
    #endregion
    #endregion

    #region Equality
    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(ReadOnlyArray<T> other) => _array == other._array;

    /// <summary>
    /// Gets a hash code representing the current instance.
    /// </summary>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public override int GetHashCode() => _array.GetHashCode();
    #endregion

    #region Containment
    /// <summary>
    /// Determines whether the <see cref="ReadOnlyArray{T}"/> contains a specific value.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public bool Contains(T item) => (ThrowIfDefault()._array as IList<T>).Contains(item);
    #endregion

    #region Conversion
    /// <summary>
    /// Converts a readonly array of type <typeparamref name="TChild"/> to a readonly array of this type,
    /// using array covariance to avoid allocating another array.
    /// </summary>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    [return: NotDefaultIfNotDefault(nameof(array))]
    public static ReadOnlyArray<T> FromChild<TChild>(ReadOnlyArray<TChild> array) where TChild : class, T
        => new(array._array);

    /// <summary>
    /// Implicitly converts an array to a readonly wrapper for it.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// </remarks>
    /// <param name="array">The array to convert.</param>
    [return: NotDefaultIfNotDefault(nameof(array))]
    public static implicit operator ReadOnlyArray<T>(T[]? array) => new(array);

    /// <summary>
    /// Wraps the array in a <see cref="ReadOnlyCollection{T}"/>.
    /// </summary>
    /// <remarks>
    /// The array will not be copied; however, it will be solely read-only accessible through the return value of
    /// this method.
    /// </remarks>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public ObjectModel.ReadOnlyCollection<T> ToReadOnlyCollection() => new(ThrowIfDefault()._array);
    #endregion

    #region Clone
    /// <summary>
    /// Gets a shallow copy of the array wrapped in this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public T[] GetClone() => Unsafe.As<T[]>(ThrowIfDefault()._array.Clone());
    #endregion

    #region Helpers
    /// <summary>
    /// Gets the item at the given index.
    /// </summary>
    /// <remarks>
    /// This method is merely for code reuse purposes.
    /// </remarks>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    private T GetAtIndex(int index) => ThrowIfDefault()._array[index];

    /// <summary>
    /// Gets the number of elements in this instance.
    /// </summary>
    /// <remarks>
    /// This method is merely for code reuse purposes.
    /// </remarks>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DoesNotReturnIfInstanceDefault]
    private int GetCount() => ThrowIfDefault()._array.Length;

    /// <summary>
    /// Throws a <see cref="DefaultInstanceException"/> if this instance is the default.
    /// </summary>
    /// <returns>This instance.</returns>
    /// <exception cref="DefaultInstanceException">Thrown if this instance is the default.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyArray<T> ThrowIfDefault() => IsDefault ? throw new DefaultInstanceException() : this;
    #endregion
    #endregion
}
