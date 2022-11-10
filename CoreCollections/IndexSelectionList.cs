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
/// A simple concrete <see cref="IndexSelectionList{T}"/> implementation defined by a selector function that is passed
/// into the constructor.
/// </summary>
/// <inheritdoc/>
public class IndexSelectorList<T> : IndexSelectionList<T>
{
    /// <summary>
    /// Gets the function that will be used to select the elements of the list.
    /// </summary>
    public Func<int, T> Selector { get; }

    /// <summary>
    /// Constructs a new instance of the <see cref="IndexSelectorList{T}"/> class defined by the selector and
    /// count passed in.
    /// </summary>
    /// <param name="Selector">The selector that will be used to get the elements of the list by index.</param>
    /// <inheritdoc cref="IndexSelectionList{T}.IndexSelectionList(int)"/>
    public IndexSelectorList(
        Func<int, T> Selector,
#pragma warning disable IDE0079 // Suppression is necessary for packaging
#pragma warning disable CS1573 // Parameter doc is inherited
        [NonNegative] int Count)
#pragma warning restore IDE0079, CS1573
        : base(Count)
    {
        this.Selector = Selector;
    }

    /// <inheritdoc/>
    protected sealed override T GetElementAt([NonNegative] int index) => Selector(index);
}

/// <summary>
/// A list defined by mapping a range starting at 0 and ending at <see cref="IndexSelectionList{T}.Count"/> to values.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public abstract class IndexSelectionList<T> : IReadOnlyList<T>
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
    /// Constructs a new instance of the <see cref="IndexSelectionList{T}"/> class with the given count.
    /// </summary>
    /// <param name="Count">The number of elements in the list.</param>
    /// <exception cref="IndexOutOfRangeException"><paramref name="Count"/> was negative.</exception>
    protected IndexSelectionList([NonNegative] int Count)
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
    /// Determines if this instance is sequence-equal to another <see cref="IndexSelectionList{T}"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool SequenceEqual(IndexSelectionList<T>? other) => SequenceEqual(other, EqualityComparer<T>.Default);

    /// <summary>
    /// Determines if this instance is sequence-equal to another <see cref="IndexSelectionList{T}"/>, using the specified
    /// <see cref="IEqualityComparer{T}"/> to compare equality of the elements.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="elementComparer">
    /// The equality comparer to use to compare equality for the elements, or <see langword="null"/> to use the
    /// default comparer for type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    public bool SequenceEqual(IndexSelectionList<T>? other, IEqualityComparer<T>? elementComparer)
    {
        if (other is null) return false;
        if (Count != other.Count) return false;

        elementComparer ??= EqualityComparer<T>.Default;
        for (int i = 0; i < Count; i++)
        {
            if (!elementComparer.Equals(GetElementAt(i), other.GetElementAt(i))) return false;
        }
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
    /// <param name="elementComparer">
    /// The equality comparer to use to compare equality for the elements, or <see langword="null"/> to use the
    /// default comparer for type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    public bool SequenceEqual(IEnumerable<T>? other, IEqualityComparer<T>? elementComparer)
    {
        if (other is null) return false;

        elementComparer ??= EqualityComparer<T>.Default;

        var otherEnumerator = other.GetEnumerator();
        for (int i = 0; i < Count; i++)
        {
            if (!otherEnumerator.MoveNext()) return false; // other is shorter than this
            if (!elementComparer.Equals(otherEnumerator.Current, GetElementAt(i))) return false; // element mismatch
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
    /// <param name="elementComparer">
    /// The equality comparer to use to compare equality for the elements, or <see langword="null"/> to use the
    /// default comparer for type <typeparamref name="T"/>.
    /// </param>
    /// <returns></returns>
    public int GetSequenceHashCode(IEqualityComparer<T>? elementComparer)
    {
        elementComparer ??= EqualityComparer<T>.Default;

        var hashCode = new HashCode();
        foreach (var item in this) hashCode.Add(item, elementComparer);
        return hashCode.ToHashCode();
    }
    #endregion
    #endregion

    #region Types
    /// <summary>
    /// An enumerator for the <see cref="IndexSelectionList{T}"/> class.
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

        private readonly IndexSelectionList<T> _list;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(IndexSelectionList<T> list)
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
