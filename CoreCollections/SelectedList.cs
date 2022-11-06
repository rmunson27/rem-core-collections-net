using Rem.Core.Attributes;
using Rem.Core.Collections.Enumeration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// A simple concrete <see cref="SelectedList{T}"/> implementation defined by a selector function.
/// </summary>
/// <inheritdoc/>
public sealed class FuncSelectedList<T> : SelectedList<T>
{
    /// <summary>
    /// Gets the function that will be used to select the elements of the list.
    /// </summary>
    public Func<int, T> Selector { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="FuncSelectedList{T}"/> class defined by the selector and
    /// count passed in.
    /// </summary>
    /// <param name="Selector">The selector that will be used to get the elements of the list by index.</param>
    /// <inheritdoc cref="SelectedList{T}.SelectedList(int)"/>
    public FuncSelectedList(Func<int, T> Selector, [NonNegative] int Count) : base(Count)
    {
        this.Selector = Selector;
    }

    /// <inheritdoc/>
    protected override T GetElementAt([NonNegative] int index) => Selector(index);
}

/// <summary>
/// A list defined by a selector applied to a range starting at 0 and ending at <see cref="SelectedList{T}.Count"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public abstract class SelectedList<T> : IReadOnlyList<T>
{
    #region Properties, Fields And Indexers
    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> was out of range.</exception>
    public T this[int index]
    {
        get
        {
            if (index < 0) throw new IndexOutOfRangeException("Index was negative.");
            else if (index >= Count) throw new IndexOutOfRangeException("Index was out of range of the list.");
            else return GetElementAt(index);
        }
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="SelectedList{T}"/> class with the given count.
    /// </summary>
    /// <param name="Count">The number of elements in the list.</param>
    /// <exception cref="IndexOutOfRangeException"><paramref name="Count"/> was negative.</exception>
    protected SelectedList([NonNegative] int Count)
    {
        if (Count < 0) throw new IndexOutOfRangeException(nameof(Count));
        this.Count = Count;
    }
    #endregion

    #region Methods
    #region Index Getter
    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <remarks>
    /// This function does not perform any index checking, which is left up to the indexer function.
    /// </remarks>
    /// <param name="index"></param>
    /// <returns></returns>
    protected abstract T GetElementAt([NonNegative] int index);
    #endregion

    #region IReadOnlyList
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public Enumerator GetEnumerator() => new(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        for (int i = 0; i < Count; i++) yield return GetElementAt(i);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (int i = 0; i < Count; i++) yield return GetElementAt(i);
    }
    #endregion

    #region Sequence Equality
    /// <summary>
    /// Determines if this instance is sequence-equal to another <see cref="SelectedList{T}"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SequenceEqual(SelectedList<T>? other) => SequenceEqual(other, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines if this instance is sequence-equal to another <see cref="SelectedList{T}"/>, using the specified
    /// <see cref="IEqualityComparer{T}"/> to compare equality of the elements.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public bool SequenceEqual(SelectedList<T>? other, IEqualityComparer<T>? comparer)
    {
        if (other is null) return false;
        if (Count != other.Count) return false;

        comparer ??= EqualityComparer<T>.Default;
        for (int i = 0; i < Count; i++) if (!comparer.Equals(GetElementAt(i), other.GetElementAt(i))) return false;

        return true;
    }

    /// <summary>
    /// Determines if this instance is sequence-equal to another <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SequenceEqual(IEnumerable<T>? other) => SequenceEqual(other, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines if this instance is sequence-equal to another <see cref="IEnumerable{T}"/>, using the specified
    /// <see cref="IEqualityComparer{T}"/> to compare equality of the elements.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public bool SequenceEqual(IEnumerable<T>? other, IEqualityComparer<T>? comparer)
    {
        if (other is null) return false;

        comparer ??= EqualityComparer<T>.Default;

        var otherEnumerator = other.GetEnumerator();
        for (int i = 0; i < Count; i++)
        {
            if (!otherEnumerator.MoveNext()) return false; // other is shorter than this
            if (!comparer.Equals(otherEnumerator.Current, GetElementAt(i))) return false; // element mismatch
        }
        if (otherEnumerator.MoveNext()) return false; // other is longer than this

        return true;
    }

    /// <summary>
    /// Gets a sequence-based hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public int GetSequenceHashCode() => GetSequenceHashCode(EqualityComparer<T>.Default);

    /// <summary>
    /// Gets a sequence-based hash code for the current instance, using the specified
    /// <see cref="IEqualityComparer{T}"/> to get hash codes for the elements of the list.
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public int GetSequenceHashCode(IEqualityComparer<T>? comparer)
    {
        comparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in this) hashCode.Add(item, comparer);
        return hashCode.ToHashCode();
    }
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// An enumerator for the <see cref="SelectedList{T}"/> class.
    /// </summary>
    public struct Enumerator
    {
        private const int NotStartedIndex = -1;
        private const int AlreadyFinishedIndex = -2;

        /// <inheritdoc cref="IEnumerator{T}.Current"/>
        public T Current => _currentIndex switch
        {
            NotStartedIndex => throw Enumerators.NotStartedException,
            AlreadyFinishedIndex => throw Enumerators.AlreadyFinishedException,
            _ => _list.GetElementAt(_currentIndex),
        };
        private int _currentIndex;

        private readonly SelectedList<T> _list;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(SelectedList<T> list)
        {
            _list = list;
            _currentIndex = NotStartedIndex;
        }

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public bool MoveNext()
        {
            if (_currentIndex == AlreadyFinishedIndex) return false;

            _currentIndex++;
            if (_currentIndex >= _list.Count)
            {
                _currentIndex = AlreadyFinishedIndex;
                return false;
            }
            else return true;
        }

        /// <inheritdoc cref="IEnumerator.Reset"/>
        public void Reset()
        {
            _currentIndex = NotStartedIndex;
        }
    }
    #endregion
}
