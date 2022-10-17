using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Enumeration;

/// <summary>
/// An enumerator struct that wraps at most one value.
/// </summary>
/// <remarks>
/// The default value of this struct is an empty enumerator.
/// </remarks>
/// <typeparam name="T"></typeparam>
public struct MaybeEnumerator<T>
{
    private readonly T _element;

    private StateValue _state;

    /// <summary>
    /// Gets the current element in the enumeration.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The enumeration has not yet started or is already finished.
    /// </exception>
    public T Current => _state switch
    {
        StateValue.NotStarted => throw Enumerators.NotStartedException,
        StateValue.SingleElementSelected => _element,
        _ => throw Enumerators.AlreadyFinishedException,
    };

    /// <summary>
    /// Constructs a new <see cref="MaybeEnumerator{T}"/> that is designed to enumerate the single element
    /// passed in.
    /// </summary>
    /// <param name="Element"></param>
    public MaybeEnumerator(T Element)
    {
        _element = Element;
        _state = StateValue.NotStarted;
    }

    /// <summary>
    /// Advances to the next item in the enumeration.
    /// </summary>
    /// <returns>Whether or not the single element in the enumeration is selected after this call.</returns>
    public bool MoveNext()
    {
        switch (_state)
        {
            case StateValue.NotStarted:
                _state = StateValue.SingleElementSelected;
                return true;

            case StateValue.SingleElementSelected:
                _state = StateValue.Finished;
                return false;

            default:
                return false;
        }
    }

    /// <summary>
    /// Resets the state of the enumeration.
    /// </summary>
    public void Reset()
    {
        if (_state != StateValue.Empty) _state = StateValue.NotStarted;
    }

    private enum StateValue : byte
    {
        Empty,
        NotStarted,
        SingleElementSelected,
        Finished,
    }
}

/// <summary>
/// An enumerator struct that wraps a single value.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct SingletonEnumerator<T>
{
    private readonly T _element;

    private StateValue _state;

    /// <summary>
    /// Gets the current element in the enumeration.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The enumeration has not yet started or is already finished.
    /// </exception>
    public T Current => _state switch
    {
        StateValue.NotStarted => throw Enumerators.NotStartedException,
        StateValue.SingleElementSelected => _element,
        _ => throw Enumerators.AlreadyFinishedException,
    };

    /// <summary>
    /// Constructs a new <see cref="SingletonEnumerator{T}"/> that is designed to enumerate the single element
    /// passed in.
    /// </summary>
    /// <param name="Element"></param>
    public SingletonEnumerator(T Element)
    {
        _element = Element;
        _state = StateValue.NotStarted;
    }

    /// <summary>
    /// Advances to the next item in the enumeration.
    /// </summary>
    /// <returns>Whether or not the single element in the enumeration is selected after this call.</returns>
    public bool MoveNext()
    {
        switch (_state)
        {
            case StateValue.NotStarted:
                _state = StateValue.SingleElementSelected;
                return true;

            case StateValue.SingleElementSelected:
                _state = StateValue.Finished;
                return false;

            default:
                return false;
        }
    }

    /// <summary>
    /// Resets the state of the enumeration.
    /// </summary>
    public void Reset()
    {
        _state = StateValue.NotStarted;
    }

    private enum StateValue : byte
    {
        NotStarted,
        SingleElementSelected,
        Finished,
    }
}

