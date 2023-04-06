using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Internal;

/// <summary>
/// Internal extensions for numeric types.
/// </summary>
internal static class NumericExtensions
{
    /// <summary>
    /// Clamps the current <see cref="long"/> to the range indicated by the inclusive minimum and maximum
    /// values specified.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Clamp(this long value, long min, long max) => Math.Min(Math.Max(value, min), max);

}
