using Rem.Core.Attributes;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D;

/// <summary>
/// Static functionality for the generic <see cref="Readonly2DArray{T}"/> struct.
/// </summary>
public static class ReadOnly2DArray
{
    /// <summary>
    /// Wraps the current array in a readonly wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Array"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="Array"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    public static ReadOnly2DArray<T> AsReadOnly<T>(this T[,] Array) => new(Array);

    /// <summary>
    /// Creates a new <see cref="ReadOnly2DArray{T}"/> wrapping a shallow copy of the specified array.
    /// </summary>
    /// <param name="Array"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="Array"/> was <see langword="null"/>.</exception>
    [return: NonDefaultableStruct]
    public static ReadOnly2DArray<T> Clone<T>(T[,] Array)
    {
        if (Array is null) throw new ArgumentNullException(nameof(Array));
        return new(Unsafe.As<T[,]>(Array.Clone()));
    }
}

/// <summary>
/// A readonly wrapper for a 2-dimensional array.
/// </summary>
/// <typeparam name="T">The type of elements of the array.</typeparam>
public readonly struct ReadOnly2DArray<T> : IDefaultableStruct, IEnumerable<T>
{
    /// <inheritdoc/>
    public bool IsDefault => _array is null;

    /// <summary>
    /// The array wrapped by this instance.
    /// </summary>
    internal readonly T[,] _array;

    /// <inheritdoc cref="Array.Length"/>
    [DoesNotReturnIfInstanceDefault]
    public int Length => _array.Length;

    /// <inheritdoc cref="Array.LongLength"/>
    [DoesNotReturnIfInstanceDefault]
    public long LongLength => _array.LongLength;

    /// <summary>
    /// Gets the element specified by the indices.
    /// </summary>
    /// <param name="index0"></param>
    /// <param name="index1"></param>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[int index0, int index1] => ref _array[index0, index1];

    /// <inheritdoc cref="Array.GetLength(int)"/>
    [DoesNotReturnIfInstanceDefault]
    public int GetLength(int dimension) => _array.GetLength(dimension);

    /// <inheritdoc cref="Array.GetLongLength(int)"/>
    [DoesNotReturnIfInstanceDefault]
    public long GetLongLength(int dimension) => _array.GetLongLength(dimension);

    [DoesNotReturnIfInstanceDefault]
    IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();

    [DoesNotReturnIfInstanceDefault]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => (_array as IEnumerable<T>)!.GetEnumerator();

    /// <summary>
    /// Constructs a new <see cref="Readonly2DArray{TElement}"/> wrapping the array passed in.
    /// </summary>
    /// <param name="Array"></param>
    /// <exception cref="ArgumentNullException"><paramref name="Array"/> was <see langword="null"/>.</exception>
    public Readonly2DArray(T[,] Array)
    {
        if (Array is null) throw new ArgumentNullException(nameof(Array));
        _array = Array;
    }
}
