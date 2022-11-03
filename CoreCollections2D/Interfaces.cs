using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D;

/// <summary>
/// Represents a read-only 2-dimensional list.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadOnlyList2D<out T> : IReadOnlyCollection2D<T>
{
    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public T this[int rowIndex, int columnIndex] { get; }

    /// <summary>
    /// Gets the rows of the collection.
    /// </summary>
    /// <returns></returns>
    public new IReadOnlyList<IReadOnlyList<T>> Rows { get; }

    /// <summary>
    /// Gets the columns of the collection.
    /// </summary>
    public new IReadOnlyList<IReadOnlyList<T>> Columns { get; }
}

/// <summary>
/// Represents a read-only 2-dimensional collection.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IReadOnlyCollection2D<out T> : IEnumerable2D<T>
{
    /// <summary>
    /// Gets the total count of elements in the collection.
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Gets the number of rows in the collection.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the number of columns in the collection.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the rows of the collection.
    /// </summary>
    /// <returns></returns>
    public new IReadOnlyCollection<IReadOnlyCollection<T>> Rows { get; }

    /// <summary>
    /// Gets the columns of the collection.
    /// </summary>
    public new IReadOnlyCollection<IReadOnlyCollection<T>> Columns { get; }
}

/// <summary>
/// Represents a 2-dimensional enumerable with enumerable rows and columns.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEnumerable2D<out T>
{
    /// <summary>
    /// Gets the rows of the enumerable.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IEnumerable<T>> Rows { get; }

    /// <summary>
    /// Gets the columns of the enumerable.
    /// </summary>
    public IEnumerable<IEnumerable<T>> Columns { get; }
}
