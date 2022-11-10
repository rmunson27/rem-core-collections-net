using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Extension methods for the <see cref="IEqualityComparer{T}"/> interface.
/// </summary>
internal static class EqualityComparerExtensions
{
    /// <summary>
    /// Gets the <see cref="IEqualityComparer{T}"/> passed in, or the default equality comparer for
    /// <typeparamref name="T"/> if <see langword="null"/> is passed in.
    /// </summary>
    /// <remarks>
    /// This method can be used to allow methods to include default <see langword="null"/> values for
    /// <see cref="IEqualityComparer{T}"/> parameters, where the <see langword="null"/> value implies that
    /// <see cref="EqualityComparer{T}.Default"/> should be used.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="comparer"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEqualityComparer<T> DefaultIfNull<T>(this IEqualityComparer<T>? comparer)
        => comparer ?? EqualityComparer<T>.Default;
}
