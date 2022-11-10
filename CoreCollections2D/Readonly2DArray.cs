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
/// Static functionality for the generic <see cref="ReadOnly2DArray{T}"/> struct.
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
public readonly record struct ReadOnly2DArray<T> : IDefaultableStruct, IReadOnlyList2D<T>
{
    #region Properties And Fields
    /// <inheritdoc/>
    public bool IsDefault => _array is null;

    /// <summary>
    /// The array wrapped by this instance.
    /// </summary>
    internal readonly T[,] _array;

    /// <summary>
    /// Gets an integer indicating the number of elements in all dimensions of the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public int Count => _array.ThrowDefaultIfNull().Length;

    /// <summary>
    /// Gets a 64-bit integer indicating the number of elements in all dimensions of the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public long LongCount => _array.ThrowDefaultIfNull().LongLength;

    /// <inheritdoc/>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    public int RowCount => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Row);

    /// <inheritdoc/>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    public int ColumnCount => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Column);

    /// <summary>
    /// Gets a 64-bit integer indicating the number of rows in the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    public long LongRowCount => _array.ThrowDefaultIfNull().GetLongLength(0);

    /// <summary>
    /// Gets a 64-bit integer indicating the number of columns in the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    public long LongColumnCount => _array.ThrowDefaultIfNull().GetLongLength(1);

    IReadOnlyList<IReadOnlyList<T>> IReadOnlyList2D<T>.Rows => Rows;

    IReadOnlyList<IReadOnlyList<T>> IReadOnlyList2D<T>.Columns => Columns;

    IReadOnlyCollection<IReadOnlyCollection<T>> IReadOnlyCollection2D<T>.Rows => Rows;

    IReadOnlyCollection<IReadOnlyCollection<T>> IReadOnlyCollection2D<T>.Columns => Columns;

    IEnumerable<IEnumerable<T>> IEnumerable2D<T>.Rows => Rows;

    IEnumerable<IEnumerable<T>> IEnumerable2D<T>.Columns => Columns;

    /// <summary>
    /// Gets the rows of the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public RowList Rows => new(in this);

    /// <summary>
    /// Gets the columns of the array.
    /// </summary>
    /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public ColumnList Columns => new(in this);

    [DoesNotReturnIfInstanceDefault]
    T IReadOnlyList2D<T>.this[int rowIndex, int columnIndex] => this[rowIndex, columnIndex];

    /// <summary>
    /// Gets the element specified by the indices.
    /// </summary>
    /// <param name="index0"></param>
    /// <param name="index1"></param>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public ref readonly T this[int index0, int index1] => ref _array.ThrowDefaultIfNull()[index0, index1];
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new <see cref="ReadOnly2DArray{TElement}"/> wrapping the array passed in.
    /// </summary>
    /// <param name="Array"></param>
    /// <exception cref="ArgumentNullException"><paramref name="Array"/> was <see langword="null"/>.</exception>
    public ReadOnly2DArray(T[,] Array)
    {
        if (Array is null) throw new ArgumentNullException(nameof(Array));
        _array = Array;
    }
    #endregion

    #region Methods
    #region Equality
    /// <summary>
    /// Determines if this <see cref="ReadOnly2DArray{T}"/> wraps <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T[,]? other) => _array == other;

    /// <summary>
    /// Determines if this <see cref="ReadOnly2DArray{T}"/> wraps the same array as <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ReadOnly2DArray<T> other) => _array == other._array;

    /// <summary>
    /// Gets a hash code representing the array wrapped in the current instance.
    /// </summary>
    /// <returns></returns>
    [DoesNotReturnIfInstanceDefault]
    public override int GetHashCode() => _array.GetHashCode();
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string representing the current instance.
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => _array?.ToString();
    #endregion

    #region Clone
    /// <summary>
    /// Gets a shallow copy of the array wrapped in this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
    [DoesNotReturnIfInstanceDefault]
    public T[,] GetClone() => Unsafe.As<T[,]>(_array.ThrowDefaultIfNull().Clone());
    #endregion
    #endregion

    #region Types
    #region Rows
    /// <summary>
    /// Represents the list of rows of a readonly 2-dimensional array.
    /// </summary>
    public readonly struct RowList : IDefaultableStruct, IReadOnlyList<Row>, IReadOnlyList<IReadOnlyList<T>>
    {
        /// <inheritdoc/>
        public bool IsDefault => _array is null;

        /// <summary>
        /// Gets the number of rows in the array as a 64-bit integer.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public long LongCount => _array.ThrowDefaultIfNull().GetLongLength(Dimension2D.Row);

        /// <summary>
        /// Gets the number of rows in the array.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public int Count => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Row);

        [DoesNotReturnIfInstanceDefault]
        IReadOnlyList<T> IReadOnlyList<IReadOnlyList<T>>.this[int index] => this[index];

        /// <summary>
        /// Gets the row at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
        /// <exception cref="IndexOutOfRangeException">The index was out of range.</exception>
        [DoesNotReturnIfInstanceDefault]
        public Row this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException("Index was negative.");
                else if (index >= Count) throw new IndexOutOfRangeException("Index exceeded maximum row index.");
                else return new(_array.ThrowDefaultIfNull(), index);
            }
        }

        private readonly T[,] _array;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RowList([NonDefaultableStruct] in ReadOnly2DArray<T> array)
        {
            _array = array._array.ThrowDefaultIfNull();
        }

        /// <summary>
        /// Gets an object that enumerates through the rows of the array.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        [return: NonDefaultableStruct] public Enumerator GetEnumerator() => new(_array);

        [DoesNotReturnIfInstanceDefault]
        IEnumerator<IReadOnlyList<T>> IEnumerable<IReadOnlyList<T>>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ri = 0L; ri < longCount; ri++) yield return new Row(_array, ri);
        }

        [DoesNotReturnIfInstanceDefault]
        IEnumerator<ReadOnly2DArray<T>.Row> IEnumerable<ReadOnly2DArray<T>.Row>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ri = 0L; ri < longCount; ri++) yield return new(_array, ri);
        }

        [DoesNotReturnIfInstanceDefault]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ri = 0L; ri < longCount; ri++) yield return new Row(_array, ri);
        }

        /// <summary>
        /// An enumerator for the <see cref="RowList"/> struct.
        /// </summary>
        public struct Enumerator : IDefaultableStruct
        {
            /// <inheritdoc/>
            public bool IsDefault => _array is null;

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public ReadOnly2DArray<T>.Row Current => _currentRowIndex switch
            {
                < 0 => throw Array2DEnumerators.NegativeIndexException(_currentRowIndex),
                _ => new(_array, _currentRowIndex),
            };

            /// <summary>
            /// The array being wrapped.
            /// </summary>
            private readonly T[,] _array;

            /// <summary>
            /// The index of the current row.
            /// </summary>
            private long _currentRowIndex;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[,] array)
            {
                _array = array;
                Array2DEnumerators.Initialize(array, Dimension2D.Row, out _currentRowIndex);
            }

            /// <inheritdoc cref="IEnumerator.Reset"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public void Reset() => Array2DEnumerators.Reset(_array, Dimension2D.Row, ref _currentRowIndex);

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public bool MoveNext() => Array2DEnumerators.MoveNext(_array, Dimension2D.Row, ref _currentRowIndex);
        }
    }

    /// <summary>
    /// Represents a row of a <see cref="ReadOnly2DArray{T}"/>.
    /// </summary>
    public readonly struct Row : IDefaultableStruct, IReadOnlyList<T>
    {
        /// <inheritdoc/>
        public bool IsDefault => _array is null;

        /// <summary>
        /// The array being wrapped.
        /// </summary>
        private readonly T[,] _array;

        /// <summary>
        /// Gets the index of the row being accessed.
        /// </summary>
        public long Index { get; }

        /// <summary>
        /// Gets the number of elements in the row.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public int Count => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Column);

        /// <summary>
        /// Gets the number of elements in the row as a 64-bit integer.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public long LongCount => _array.ThrowDefaultIfNull().GetLongLength(Dimension2D.Column);

        /// <inheritdoc/>
        /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public ref readonly T this[long index] => ref _array.ThrowDefaultIfNull()[Index, index];

        [DoesNotReturnIfInstanceDefault]
        T IReadOnlyList<T>.this[int index] => _array.ThrowDefaultIfNull()[Index, index];

        /// <summary>
        /// Constructs a new <see cref="Row"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        internal Row(T[,] array, long index)
        {
            _array = array;
            Index = index;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public Enumerator GetEnumerator() => new(in this);

        [DoesNotReturnIfInstanceDefault]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (long ci = 0; ci < longCount; ci++)
            {
                yield return _array[Index, ci];
            }
        }

        [DoesNotReturnIfInstanceDefault]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (long ci = 0; ci < longCount; ci++)
            {
                yield return _array[Index, ci];
            }
        }

        /// <summary>
        /// An enumerator for the <see cref="Row"/> struct.
        /// </summary>
        public struct Enumerator : IDefaultableStruct
        {
            /// <inheritdoc/>
            public bool IsDefault => _array is null;

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public T Current => _currentColumnIndex switch
            {
                < 0 => throw Array2DEnumerators.NegativeIndexException(_currentColumnIndex),
                _ => _array[RowIndex, _currentColumnIndex],
            };

            /// <summary>
            /// Gets the index of the row being enumerated.
            /// </summary>
            private long RowIndex { get; }

            /// <summary>
            /// Gets the index of the column of the current element of the enumeration.
            /// </summary>
            private long _currentColumnIndex;

            private readonly T[,] _array;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator([NonDefaultableStruct] in Row row)
            {
                _array = row._array;
                RowIndex = row.Index;
                Array2DEnumerators.Initialize(row._array, Dimension2D.Column, out _currentColumnIndex);
            }

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public bool MoveNext() => Array2DEnumerators.MoveNext(_array, Dimension2D.Column, ref _currentColumnIndex);

            /// <inheritdoc cref="IEnumerator.Reset"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [InstanceNotDefault]
            public void Reset() => Array2DEnumerators.Reset(_array, Dimension2D.Column, ref _currentColumnIndex);
        }
    }
    #endregion

    #region Columns
    /// <summary>
    /// Represents the list of columns of a readonly 2-dimensional array.
    /// </summary>
    public readonly struct ColumnList : IDefaultableStruct, IReadOnlyList<Column>, IReadOnlyList<IReadOnlyList<T>>
    {
        /// <inheritdoc/>
        public bool IsDefault => _array is null;

        /// <summary>
        /// Gets the number of columns in the array as a 64-bit integer.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public long LongCount => _array.ThrowDefaultIfNull().GetLongLength(Dimension2D.Column);

        /// <summary>
        /// Gets the number of columns in the array.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public int Count => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Column);

        [DoesNotReturnIfInstanceDefault]
        IReadOnlyList<T> IReadOnlyList<IReadOnlyList<T>>.this[int index] => this[index];

        /// <summary>
        /// Gets the column at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
        /// <exception cref="IndexOutOfRangeException">The index was out of range.</exception>
        [DoesNotReturnIfInstanceDefault]
        public ReadOnly2DArray<T>.Column this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException("Index was negative.");
                else if (index >= Count) throw new IndexOutOfRangeException("Index exceeded maximum column index.");
                else return new(_array.ThrowDefaultIfNull(), index);
            }
        }

        private readonly T[,] _array;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ColumnList([NonDefaultableStruct] in ReadOnly2DArray<T> array)
        {
            _array = array._array.ThrowDefaultIfNull();
        }

        /// <summary>
        /// Gets an object that enumerates through the columns of the array.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        [return: NonDefaultableStruct] public Enumerator GetEnumerator() => new(_array);

        [DoesNotReturnIfInstanceDefault]
        IEnumerator<ReadOnly2DArray<T>.Column> IEnumerable<ReadOnly2DArray<T>.Column>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ci = 0L; ci < longCount; ci++) yield return new(_array, ci);
        }

        [DoesNotReturnIfInstanceDefault]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ci = 0L; ci < longCount; ci++) yield return new Column(_array, ci);
        }

        [DoesNotReturnIfInstanceDefault]
        IEnumerator<IReadOnlyList<T>> IEnumerable<IReadOnlyList<T>>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (var ci = 0L; ci < longCount; ci++) yield return new Column(_array, ci);
        }

        /// <summary>
        /// An enumerator for the <see cref="ColumnList"/> struct.
        /// </summary>
        public struct Enumerator : IDefaultableStruct
        {
            /// <inheritdoc/>
            public bool IsDefault => _array is null;

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public ReadOnly2DArray<T>.Column Current => _currentColumnIndex switch
            {
                < 0 => throw Array2DEnumerators.NegativeIndexException(_currentColumnIndex),
                _ => new(_array, _currentColumnIndex),
            };

            /// <summary>
            /// The array being wrapped.
            /// </summary>
            private readonly T[,] _array;

            /// <summary>
            /// The index of the current row.
            /// </summary>
            private long _currentColumnIndex;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[,] array)
            {
                _array = array;
                Array2DEnumerators.Initialize(array, Dimension2D.Column, out _currentColumnIndex);
            }

            /// <inheritdoc cref="IEnumerator.Reset"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public void Reset() => Array2DEnumerators.Reset(_array, Dimension2D.Column, ref _currentColumnIndex);

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public bool MoveNext() => Array2DEnumerators.MoveNext(_array, Dimension2D.Column, ref _currentColumnIndex);
        }
    }

    /// <summary>
    /// Represents a column of a <see cref="ReadOnly2DArray{T}"/>.
    /// </summary>
    public readonly struct Column : IDefaultableStruct, IReadOnlyList<T>
    {
        /// <summary>
        /// Gets whether or not this instance is the default.
        /// </summary>
        public bool IsDefault => _array is null;

        /// <summary>
        /// The array being wrapped.
        /// </summary>
        private readonly T[,] _array;

        /// <summary>
        /// Gets the index of the column being accessed.
        /// </summary>
        public long Index { get; }

        /// <summary>
        /// Gets the number of elements in the column.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public int Count => _array.ThrowDefaultIfNull().GetLength(Dimension2D.Row);

        /// <summary>
        /// Gets the number of elements in the column as a 64-bit integer.
        /// </summary>
        /// <exception cref="DefaultInstanceException">This property was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public long LongCount => _array.ThrowDefaultIfNull().GetLongLength(Dimension2D.Row);

        /// <inheritdoc/>
        /// <exception cref="DefaultInstanceException">This indexer was accessed on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public ref readonly T this[int index] => ref _array.ThrowDefaultIfNull()[index, Index];

        [DoesNotReturnIfInstanceDefault]
        T IReadOnlyList<T>.this[int index] => _array[index, Index];

        /// <summary>
        /// Constructs a new <see cref="Column"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Column(T[,] array, long index)
        {
            _array = array;
            Index = index;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
        [DoesNotReturnIfInstanceDefault]
        public Enumerator GetEnumerator() => new(in this);

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (long ri = 0; ri < longCount; ri++)
            {
                yield return _array[ri, Index];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (IsDefault) throw new DefaultInstanceException();

            var longCount = LongCount;
            for (long ri = 0; ri < longCount; ri++)
            {
                yield return _array[ri, Index];
            }
        }

        /// <summary>
        /// An enumerator for the <see cref="Column"/> struct.
        /// </summary>
        public struct Enumerator : IDefaultableStruct
        {
            /// <inheritdoc/>
            public bool IsDefault => _array is null;

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public T Current => _currentRowIndex switch
            {
                < 0 => throw Array2DEnumerators.NegativeIndexException(_currentRowIndex),
                _ => _array[_currentRowIndex, ColumnIndex],
            };

            /// <summary>
            /// Gets the index of the column being enumerated.
            /// </summary>
            private long ColumnIndex { get; }

            /// <summary>
            /// The index of the row of the current element of the enumeration.
            /// </summary>
            private long _currentRowIndex;

            private readonly T[,] _array;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator([NonDefaultableStruct] in Column column)
            {
                _array = column._array;
                ColumnIndex = column.Index;
                Array2DEnumerators.Initialize(column._array, Dimension2D.Row, out _currentRowIndex);
            }

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public bool MoveNext() => Array2DEnumerators.MoveNext(_array, Dimension2D.Row, ref _currentRowIndex);

            /// <inheritdoc cref="IEnumerator.Reset"/>
            /// <exception cref="DefaultInstanceException">This method was called on the default.</exception>
            [DoesNotReturnIfInstanceDefault]
            public void Reset() => Array2DEnumerators.Reset(_array, Dimension2D.Row, ref _currentRowIndex);
        }
    }
    #endregion
    #endregion
}
