using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;

/// <summary>
/// Values and value generation methods used for testing.
/// </summary>
public static class Values
{
    /// <summary>
    /// Creates an array of indices spanning the dimensions passed in.
    /// </summary>
    /// <param name="Dimension0"></param>
    /// <param name="Dimension1"></param>
    /// <returns></returns>
    public static (long Index0, long Index1)[,] Indices(long Dimension0, long Dimension1)
    {
        var arr = new (long, long)[Dimension0, Dimension1];
        for (long i0 = 0; i0 < Dimension0; i0++)
        {
            for (long i1 = 0; i1 < Dimension1; i1++)
            {
                arr[i0, i1] = (i0, i1);
            }
        }

        return arr;
    }

    /// <summary>
    /// Creates a new 2-dimensional array from the index selection operation passed in.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="Count0"></param>
    /// <param name="Count1"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static TElement[,] Create<TElement>(
        long Count0, long Count1, Func<long, long, TElement> selector)
    {
        var arr = new TElement[Count0, Count1];
        for (int i0 = 0; i0 < Count0; i0++)
        {
            for (int i1 = 0; i1 < Count1; i1++)
            {
                arr[i0, i1] = selector(i0, i1);
            }
        }

        return arr;
    }
}
