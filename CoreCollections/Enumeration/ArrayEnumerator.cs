using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Enumeration;

/// <summary>
/// An enumerator structure for an array of a specified type.
/// </summary>
/// <remarks>
/// This can be used to avoid creating an object when only a struct is needed for iterating through an array.
/// </remarks>
/// <typeparam name="TElement">The type of elements of the array.</typeparam>
public struct ArrayEnumerator<TElement>
{
    /// <summary>
    /// Gets the index of the current element.
    /// </summary>
    public long CurrentIndex => _currentIndex;

    /// <inheritdoc cref="IEnumerator{T}.Current"/>
    public TElement Current
    {
        get
        {
            if (_currentIndex < 0) throw Enumerators.NotStartedException;
            else if (_currentIndex >= _array.LongLength) throw Enumerators.AlreadyFinishedException;
            else return _array[_currentIndex];
        }
    }

    /// <summary>
    /// The index of the current element.
    /// </summary>
    private long _currentIndex;

    /// <summary>
    /// The array being enumerated.
    /// </summary>
    private readonly TElement[] _array;

    /// <summary>
    /// Constructs a new <see cref="ArrayEnumerator{TElement}"/> wrapping the array passed in.
    /// </summary>
    /// <param name="array"></param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> was <see langword="null"/>.</exception>
    public ArrayEnumerator(TElement[] array)
    {
        _array = array;
        _currentIndex = -1;
    }

    /// <inheritdoc cref="IEnumerator.MoveNext"/>
    public bool MoveNext()
    {
        if (_array.LongLength == 0) return false;

        if (_currentIndex < _array.LongLength) _currentIndex++;
        return _currentIndex < _array.LongLength;
    }

    /// <inheritdoc cref="IEnumerator.Reset"/>
    public void Reset()
    {
        _currentIndex = -1;
    }
}

