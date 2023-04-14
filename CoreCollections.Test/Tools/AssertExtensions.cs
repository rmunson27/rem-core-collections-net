using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test.Tools;

/// <summary>
/// Extension methods for the <see cref="Assert"/> class.
/// </summary>
internal static class AssertExtensions
{
    /// <summary>
    /// Asserts that the given function does not throw an exception, returning the return value of the function if not.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the function.</typeparam>
    /// <param name="_"></param>
    /// <param name="func"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="AssertFailedException"></exception>
    public static TReturn DoesNotThrow<TReturn>(this Assert _, Func<TReturn> func, string message = "")
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            throw new AssertFailedException(
                    $"Exception of type {ex.GetType()} with message \"{ex.Message}\" thrown."
                        + (string.IsNullOrWhiteSpace(message) ? "" : $" {message}"),
                    ex);
        }
    }

    /// <summary>
    /// Asserts that the two sequences have equal lengths.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="_"></param>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreSequenceEqual<TSource>(this Assert _,
                                                 IEnumerable<TSource>? expected, IEnumerable<TSource>? actual,
                                                 IEqualityComparer<TSource>? comparer = null,
                                                 bool showCollections = false,
                                                 string message = "")
    {
        string ShownCollectionString(IEnumerable<TSource>? collection)
        {
            if (showCollections) return " " + CollectionString(collection);
            else return "";
        }

        static string CollectionString(IEnumerable<TSource>? collection)
            => collection is null
                ? "null"
                : $"{{ {string.Join(", ", collection)} }}";

        if (expected is null)
        {
            Assert.IsNull(
                actual,
                CombineMessages(
                    $"Expected sequence was null but actual sequence{ShownCollectionString(actual)} was not.",
                    message));
        }
        else if (actual is null)
        {
            Assert.IsNull(
                expected,
                CombineMessages(
                    $"Expected sequence{ShownCollectionString(expected)} was not null but actual sequence was.",
                    message));
        }
        else
        {
            using var expectedEnumerator = expected.GetEnumerator();
            using var actualEnumerator = actual.GetEnumerator();

            bool expectedHasNext = expectedEnumerator.MoveNext(), actualHasNext = actualEnumerator.MoveNext();
            long index = 0;
            while (expectedHasNext && actualHasNext)
            {
                Assert.AreEqual(expectedEnumerator.Current, actualEnumerator.Current,
                                comparer,
                                CombineMessages(
                                    "Collections"
                                        + (showCollections
                                            ? $" {CollectionString(expected)} and {CollectionString(actual)}"
                                            : "")
                                        + $" were not equal - Elements at index {index} did not match.",
                                    message));
                (expectedHasNext, actualHasNext) = (expectedEnumerator.MoveNext(), actualEnumerator.MoveNext());
                index++;
            }

            if (expectedHasNext)
            {
                Assert.Fail(
                    CombineMessages(
                        $"Expected collection{ShownCollectionString(expected)} is longer than actual collection" +
                        $"{ShownCollectionString(actual)} length of {index}.",
                    message));
            }
            else if (actualHasNext)
            {
                Assert.Fail(
                    CombineMessages(
                        $"Actual collection{ShownCollectionString(actual)} is longer than expected collection" +
                        $"{ShownCollectionString(expected)} length of {index}.",
                    message));
            }
        }
    }

    private static string CombineMessages(string? initial, string? next)
    {
        if (string.IsNullOrEmpty(initial)) return string.IsNullOrEmpty(next) ? "" : next;
        else if (string.IsNullOrEmpty(next)) return initial;
        else
        {
            StringBuilder builder = new();
            builder.Append(initial);
            builder.Append(' ');
            builder.Append(next);
            return builder.ToString();
        }
    }
}
