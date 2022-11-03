using Rem.Core.Collections.Enumeration;
using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Helper methods for 2D array vector (row and column) enumerators.
/// </summary>
internal static class Array2DEnumerators
{
    /// <summary>
    /// An invalid array index that represents the "Not Started" enumeration state.
    /// </summary>
    public const long NotStartedIndex = -1;

    /// <summary>
    /// An invalid array index that can be used to represent the "Already Finished" enumeration state.
    /// </summary>
    public const long AlreadyFinishedIndex = -2;

    /// <summary>
    /// Gets the exception to throw for an index that indicates that the enumeration has not started or
    /// already finished.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException NegativeIndexException(long index) => index switch
    {
        NotStartedIndex => Enumerators.NotStartedException,
        _ => Enumerators.AlreadyFinishedException,
    };

    /// <summary>
    /// Performs initialization of a struct enumerator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="nonFixedDimension"></param>
    /// <param name="currentIndex"></param>
    /// <exception cref="DefaultInstanceException">
    /// <paramref name="array"/> is <see langword="null"/>, indicating a default instance.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize<T>(T[,] array, Dimension2D nonFixedDimension, out long currentIndex)
    {
        if (array.ThrowDefaultIfNull().GetLongLength(nonFixedDimension) == 0) currentIndex = AlreadyFinishedIndex;
        else currentIndex = NotStartedIndex;
    }

    /// <summary>
    /// Performs the <see cref="IEnumerator.MoveNext"/> functionality for a given 2-dimensional array and index.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="currentIndex"></param>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">
    /// <paramref name="array"/> is <see langword="null"/>, indicating a default instance.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool MoveNext<T>(T[,] array, Dimension2D nonFixedDimension, ref long currentIndex)
    {
        switch (currentIndex)
        {
            case AlreadyFinishedIndex:
                return false;

            case var x when x == array.ThrowDefaultIfNull().GetLongLength(nonFixedDimension) - 1:
                currentIndex = AlreadyFinishedIndex;
                return false;

            default:
                currentIndex++;
                return true;
        }
    }

    /// <summary>
    /// Resets the current index value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="nonFixedDimension"></param>
    /// <param name="currentIndex"></param>
    /// <exception cref="DefaultInstanceException">
    /// <paramref name="array"/> is <see langword="null"/>, indicating a default instance.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset<T>(T[,] array, Dimension2D nonFixedDimension, ref long currentIndex)
    {
        if (array.ThrowDefaultIfNull().GetLongLength(nonFixedDimension) > 0) currentIndex = NotStartedIndex;
    }

    /// <summary>
    /// Gets the array passed in, or throws an exception indicating that the enumerator instance is the default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    /// <exception cref="DefaultInstanceException">
    /// <paramref name="array"/> is <see langword="null"/>, indicating a default instance.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[,] ThrowDefaultIfNull<T>(this T[,]? array)
        => array is null ? throw new DefaultInstanceException() : array;
}
