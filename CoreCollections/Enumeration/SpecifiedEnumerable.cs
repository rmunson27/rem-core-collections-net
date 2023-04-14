using Rem.Core.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Enumeration;

/// <summary>
/// Extension methods and other static functionality for the
/// <see cref="ISpecifiedEnumerable{TEnumerator, T}"/> interface.
/// </summary>
/// <remarks>
/// The methods in this class are sufficiently generic to allow the compiler to bypass boxing the types involved, even
/// if the <see cref="ISpecifiedEnumerable{TEnumerator, T}"/> types being used are structs.
/// <para/>
/// All of the <see cref="ISpecifiedEnumerable{TEnumerator, T}"/> arguments in the arguments of methods provided by
/// this class are passed with the <see langword="in"/> keyword, so if this class is used to implement extension
/// methods for a <see langword="struct"/> <see cref="ISpecifiedEnumerable{TEnumerator, T}"/>, the
/// <see langword="struct"/> should ideally be <see langword="readonly"/> to prevent defensive copying.
/// </remarks>
public static class SpecifiedEnumerable
{
    #region LINQ
    /// <summary>
    /// Determines if the <see cref="ImmutableArray{T}"/> is sequence-equal tot he
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChildEnumerable"></typeparam>
    /// <typeparam name="TChildEnumerator"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    /// <exception cref="StructArgumentDefaultException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool SequenceEqual<TParent,
                                     TChildEnumerable, TChildEnumerator, TChild>(
        ImmutableArray<TParent> first, in TChildEnumerable second, IEqualityComparer<TParent>? comparer)
    where TChildEnumerable : ISpecifiedEnumerable<TChildEnumerator, TChild>
    where TChildEnumerator : struct, IEnumerator<TChild>
    where TChild : TParent
    {
        if (first.IsDefault) throw new StructArgumentDefaultException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));

        var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        comparer = comparer.DefaultIfNull();

        bool isFirstRunning, isSecondRunning;
        while ((isFirstRunning = firstEnumerator.MoveNext()) & (isSecondRunning = secondEnumerator.MoveNext()))
        {
            if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current)) return false;
        }

        return isFirstRunning == isSecondRunning;
    }

    /// <inheritdoc cref="SequenceEqual{TParentEnumerable, TParentEnumerator, TParent, TChildEnumerable, TChildEnumerator, TChild}(in TParentEnumerable, in TChildEnumerable, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TParent,
                                     TChildEnumerable, TChildEnumerator, TChild>(
        IEnumerable<TParent> first, in TChildEnumerable second, IEqualityComparer<TParent>? comparer)
    where TChildEnumerable : ISpecifiedEnumerable<TChildEnumerator, TChild>
    where TChildEnumerator : struct, IEnumerator<TChild>
    where TChild : TParent
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));

        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        comparer = comparer.DefaultIfNull();

        bool isFirstRunning, isSecondRunning;
        while ((isFirstRunning = firstEnumerator.MoveNext()) & (isSecondRunning = secondEnumerator.MoveNext()))
        {
            if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current)) return false;
        }

        return isFirstRunning == isSecondRunning;
    }

    /// <inheritdoc cref="SequenceEqual{TParentEnumerable, TParentEnumerator, TParent, TChildEnumerable, TChildEnumerator, TChild}(in TParentEnumerable, in TChildEnumerable, IEqualityComparer{TParent}?)"/>
    public static bool SequenceEqual<TSourceEnumerable, TSourceEnumerator, TSource>(
        in TSourceEnumerable first, IEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
    where TSourceEnumerable : ISpecifiedEnumerable<TSourceEnumerator, TSource>
    where TSourceEnumerator : struct, IEnumerator<TSource>
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));

        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        comparer = comparer.DefaultIfNull();

        bool isFirstRunning, isSecondRunning;
        while ((isFirstRunning = firstEnumerator.MoveNext()) & (isSecondRunning = secondEnumerator.MoveNext()))
        {
            if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current)) return false;
        }

        return isFirstRunning == isSecondRunning;
    }

    /// <summary>
    /// Determines if the current enumerable is sequence-equal to the other enumerable passed in.
    /// </summary>
    /// <typeparam name="TParentEnumerable">The type of the current enumerable.</typeparam>
    /// <typeparam name="TParentEnumerator">
    /// The type of the enumerator returned by the current enumerable.
    /// </typeparam>
    /// <typeparam name="TParent">The type of elements of the first enumerable.</typeparam>
    /// <typeparam name="TChildEnumerable">The type of the second enumerable.</typeparam>
    /// <typeparam name="TChildEnumerator">
    /// The type of the <see langword="struct"/> enumerator returned by the second enumerable.
    /// </typeparam>
    /// <typeparam name="TChild">
    /// The type of elements of the second enumerable, a subtype of <typeparamref name="TParent"/>.
    /// </typeparam>
    /// <param name="first">The first enumerable to compare.</param>
    /// <param name="second">The second enumerable to compare.</param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{T}"/> to use to compare instances of type <typeparamref name="TParent"/>, or
    /// <see langword="null"/> to use the default equality comparer for type <typeparamref name="TParent"/>.
    /// </param>
    /// <returns>Whether or not the two enumerables are sequence-equal.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="first"/> or <paramref name="second"/> was <see langword="null"/>.
    /// This is clearly not possible if both <typeparamref name="TParentEnumerable"/> and
    /// <typeparamref name="TChildEnumerable"/> are value types.
    /// </exception>
    public static bool SequenceEqual<TParentEnumerable, TParentEnumerator, TParent,
                                     TChildEnumerable, TChildEnumerator, TChild>(
        in TParentEnumerable first, in TChildEnumerable second, IEqualityComparer<TParent>? comparer)
    where TParentEnumerable : ISpecifiedEnumerable<TParentEnumerator, TParent>
    where TParentEnumerator : struct, IEnumerator<TParent>
    where TChildEnumerable : ISpecifiedEnumerable<TChildEnumerator, TChild>
    where TChildEnumerator : struct, IEnumerator<TChild>
    where TChild : TParent
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));

        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        comparer = comparer.DefaultIfNull();

        bool isFirstRunning, isSecondRunning;
        while ((isFirstRunning = firstEnumerator.MoveNext()) & (isSecondRunning = secondEnumerator.MoveNext()))
        {
            if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current)) return false;
        }

        return isFirstRunning == isSecondRunning;
    }
    #endregion

    #region StringJoin
    /// <inheritdoc cref="StringJoin{TEnumerable, TEnumerator, TElement}(in TEnumerable, string?)"/>
    public static string StringJoin<TEnumerable, TEnumerator, TElement>(in TEnumerable items, char separator)
    where TEnumerable : ISpecifiedEnumerable<TEnumerator, TElement>
    where TEnumerator : struct, IEnumerator<TElement>
    {
        using var enumerator = items.GetEnumerator();

        if (!enumerator.MoveNext()) return "";

        StringBuilder sb = new();
        sb.Append(enumerator.Current);

        while (enumerator.MoveNext())
        {
            sb.Append(separator);
            sb.Append(enumerator.Current);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Joins the elements of the current enumerable into a string, separating them with the specified separator.
    /// </summary>
    /// <typeparam name="TEnumerable">The type of the current enumerable.</typeparam>
    /// <typeparam name="TEnumerator">The type of the enumerator returned by the current enumerable.</typeparam>
    /// <typeparam name="TElement">The element type of the current enumerable.</typeparam>
    /// <param name="items">The current enumerable.</param>
    /// <param name="separator">The separator to write between elements of the current enumerable.</param>
    /// <returns>
    /// A <see cref="string"/> containing the formatted elements of the current enumerable, separated
    /// by <paramref name="separator"/>.
    /// </returns>
    public static string StringJoin<TEnumerable, TEnumerator, TElement>(in TEnumerable items, string? separator)
    where TEnumerable : ISpecifiedEnumerable<TEnumerator, TElement>
    where TEnumerator : struct, IEnumerator<TElement>
    {
        if (items is null) throw new ArgumentNullException(nameof(items));

        using var enumerator = items.GetEnumerator();

        if (!enumerator.MoveNext()) return "";

        StringBuilder sb = new();
        sb.Append(enumerator.Current);

        while (enumerator.MoveNext())
        {
            sb.Append(separator);
            sb.Append(enumerator.Current);
        }

        return sb.ToString();
    }
    #endregion
}

/// <summary>
/// An interface for enumerable types that produce a specified <see cref="IEnumerator{T}"/>-implementing
/// type that can be used internally in a <see langword="foreach"/> loop.
/// </summary>
/// <remarks>
/// The purpose of this interface is to provide a way to simplify creating extension methods for various
/// <see langword="struct"/> collection types that also may return <see langword="struct"/> enumerators,
/// to prevent unnecessary boxing.
/// </remarks>
public interface ISpecifiedEnumerable<TEnumerator, out T> : IEnumerable<T> where TEnumerator : IEnumerator<T>
{
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public new TEnumerator GetEnumerator();
}
