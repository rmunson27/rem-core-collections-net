using Rem.Core.Collections;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D;

/// <summary>
/// Static functionality relating to 2-dimensional arrays.
/// </summary>
public static class Array2D
{
    #region FromRows
    /// <summary>
    /// Creates a new 2-dimensional array from the rows passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rows"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the rows passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the rows passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="rows"/> was empty.
    /// </exception>
    public static T[,] FromRows<T>(params T[][] rows)
    {
        // Rows error checking
        if (rows is null) throw new ArgumentNullException(nameof(rows));
        if (rows.LongLength == 0) throw new ArgumentException("No rows were passed in.");

        // Individual row checking
        var rowLength = rows[0].LongLength;
        foreach (var row in rows.Skip(1))
        {
            if (row is null) throw new ArgumentNullException("One of the rows was null.", default(Exception));
            if (row.LongLength != rowLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[rows.LongLength, rowLength];
        for (long r = 0L; r < rows.LongLength; r++)
        {
            for (long c = 0L; c < rows[r].LongLength; c++)
            {
                result[r, c] = rows[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the rows passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rows"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the rows passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the rows passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="rows"/> was empty.
    /// </exception>
    public static T[,] FromRows<T>(params IReadOnlyList<T>[] rows)
    {
        // Rows error checking
        if (rows is null) throw new ArgumentNullException(nameof(rows));
        if (rows.LongLength == 0) throw new ArgumentException("No rows were passed in.");

        // Individual row checking
        var rowLength = rows[0].Count;
        foreach (var row in rows.Skip(1))
        {
            if (row is null) throw new ArgumentNullException("One of the rows was null.", default(Exception));
            if (row.Count != rowLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[rows.LongLength, rowLength];
        for (long r = 0L; r < rows.LongLength; r++)
        {
            for (int c = 0; c < rows[r].Count; c++)
            {
                result[r, c] = rows[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the rows passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rows"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the rows passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the rows passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="rows"/> was empty.
    /// </exception>
    public static T[,] FromRows<T>(IReadOnlyList<T[]> rows)
    {
        // Rows error checking
        if (rows is null) throw new ArgumentNullException(nameof(rows));
        if (rows.Count == 0) throw new ArgumentException("No rows were passed in.");

        // Individual row checking
        var rowLength = rows[0].LongLength;
        foreach (var row in rows.Skip(1))
        {
            if (row is null) throw new ArgumentNullException("One of the rows was null.", default(Exception));
            if (row.LongLength != rowLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[rows.Count, rowLength];
        for (int r = 0; r < rows.Count; r++)
        {
            for (long c = 0L; c < rows[r].LongLength; c++)
            {
                result[r, c] = rows[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the rows passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rows"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the rows passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the rows passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="rows"/> was empty.
    /// </exception>
    public static T[,] FromRows<T>(IReadOnlyList<IReadOnlyList<T>> rows)
    {
        // Rows error checking
        if (rows is null) throw new ArgumentNullException(nameof(rows));
        if (rows.Count == 0) throw new ArgumentException("No rows were passed in.");

        // Individual row checking
        var rowLength = rows[0].Count;
        foreach (var row in rows.Skip(1))
        {
            if (row is null) throw new ArgumentNullException("One of the rows was null.", default(Exception));
            if (row.Count != rowLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[rows.Count, rowLength];
        for (int r = 0; r < rows.Count; r++)
        {
            for (int c = 0; c < rows[r].Count; c++)
            {
                result[r, c] = rows[r][c];
            }
        }

        return result;
    }
    #endregion

    #region FromColumns
    /// <summary>
    /// Creates a new 2-dimensional array from the columns passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="columns"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the columns passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the columns passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="columns"/> was empty.
    /// </exception>
    public static T[,] FromColumns<T>(params T[][] columns)
    {
        // Columns error checking
        if (columns is null) throw new ArgumentNullException(nameof(columns));
        if (columns.LongLength == 0) throw new ArgumentException("No columns were passed in.");

        // Individual column checking
        var columnLength = columns[0].LongLength;
        foreach (var column in columns.Skip(1))
        {
            if (column is null) throw new ArgumentNullException("One of the columns was null.", default(Exception));
            if (column.LongLength != columnLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[columnLength, columns.LongLength];
        for (long c = 0L; c < columns.LongLength; c++)
        {
            for (long r = 0L; r < columnLength; r++)
            {
                result[r, c] = columns[c][r];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the columns passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="columns"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the columns passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the columns passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="columns"/> was empty.
    /// </exception>
    public static T[,] FromColumns<T>(params IReadOnlyList<T>[] columns)
    {
        // Columns error checking
        if (columns is null) throw new ArgumentNullException(nameof(columns));
        if (columns.LongLength == 0) throw new ArgumentException("No columns were passed in.");

        // Individual column checking
        var columnLength = columns[0].Count;
        foreach (var column in columns.Skip(1))
        {
            if (column is null) throw new ArgumentNullException("One of the columns was null.", default(Exception));
            if (column.Count != columnLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[columnLength, columns.LongLength];
        for (long c = 0L; c < columns.LongLength; c++)
        {
            for (int r = 0; r < columnLength; r++)
            {
                result[r, c] = columns[c][r];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the columns passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="columns"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the columns passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the columns passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="columns"/> was empty.
    /// </exception>
    public static T[,] FromColumns<T>(IReadOnlyList<T[]> columns)
    {
        // Columns error checking
        if (columns is null) throw new ArgumentNullException(nameof(columns));
        if (columns.Count == 0) throw new ArgumentException("No columns were passed in.");

        // Individual column checking
        var columnLength = columns[0].LongLength;
        foreach (var column in columns.Skip(1))
        {
            if (column is null) throw new ArgumentNullException("One of the columns was null.", default(Exception));
            if (column.LongLength != columnLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[columnLength, columns.Count];
        for (int c = 0; c < columns.Count; c++)
        {
            for (long r = 0L; r < columnLength; r++)
            {
                result[r, c] = columns[c][r];
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the columns passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="columns"/> was <see langword="null"/>
    /// <para/>
    /// OR
    /// <para/>
    /// one of the columns passed in was <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The lengths of the columns passed in were not all identical
    /// <para/>
    /// OR
    /// <para/>
    /// <paramref name="columns"/> was empty.
    /// </exception>
    public static T[,] FromColumns<T>(IReadOnlyList<IReadOnlyList<T>> columns)
    {
        // Columns error checking
        if (columns is null) throw new ArgumentNullException(nameof(columns));
        if (columns.Count == 0) throw new ArgumentException("No columns were passed in.");

        // Individual column checking
        var columnLength = columns[0].Count;
        foreach (var column in columns.Skip(1))
        {
            if (column is null) throw new ArgumentNullException("One of the columns was null.", default(Exception));
            if (column.Count != columnLength) throw new ArgumentException("Rows had mismatched lengths.");
        }

        // Build the array
        var result = new T[columnLength, columns.Count];
        for (int c = 0; c < columns.Count; c++)
        {
            for (int r = 0; r < columnLength; r++)
            {
                result[r, c] = columns[c][r];
            }
        }

        return result;
    }
    #endregion

    #region Enumerate
    /// <summary>
    /// Enumerates the elements of the 2-dimensional array passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="array"/> was default.
    /// </exception>
    public static IEnumerable<T> Enumerate<T>(ReadOnly2DArray<T> array)
    {
        if (array.IsDefault) throw new StructArgumentDefaultException(nameof(array));
        return Enumerate(array._array);
    }

    /// <summary>
    /// Enumerates the elements of the 2-dimensional array passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> was <see langword="null"/>.
    /// </exception>
    public static IEnumerable<T> Enumerate<T>(T[,] array)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        foreach (var item in array) yield return item;
    }
    #endregion

    #region GetEnumerator
    /// <summary>
    /// Gets an enumerator for the 2-dimensional array passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException">
    /// <paramref name="array"/> was default.
    /// </exception>
    public static IEnumerator<T> GetEnumerator<T>(ReadOnly2DArray<T> array)
    {
        if (array.IsDefault) throw new StructArgumentDefaultException(nameof(array));
        return GetEnumerator(array._array);
    }

    /// <summary>
    /// Gets an enumerator for the 2-dimensional array passed in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> was <see langword="null"/>.
    /// </exception>
    public static IEnumerator<T> GetEnumerator<T>(T[,] array)
    {
        if (array is null) throw new ArgumentNullException(nameof(array));
        foreach (var item in array) yield return item;
    }
    #endregion
}
