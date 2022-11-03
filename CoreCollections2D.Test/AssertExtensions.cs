using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;

/// <summary>
/// Extensions for the <see cref="Assert"/> class.
/// </summary>
internal static class AssertExtensions
{
    /// <summary>
    /// Asserts that the two sequences are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_"></param>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="comparer"></param>
    /// <param name="message"></param>
    public static void AreSequenceEqual<T>(
        this Assert _, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? comparer = null,
        string message = "")
    {
        comparer ??= EqualityComparer<T>.Default;

        var expectedEnumerator = expected.GetEnumerator();
        var actualEnumerator = actual.GetEnumerator();
        bool expectedMoveNext, actualMoveNext;
        int index = -1;

        do
        {
            expectedMoveNext = expectedEnumerator.MoveNext();
            actualMoveNext = actualEnumerator.MoveNext();
            index++;

            // Remove cases where one sequence exceeds the length of the other
            if (!expectedMoveNext)
            {
                if (actualMoveNext)
                {
                    Assert.Fail(
                        CombineMessages($"Actual sequence exceeded length of expected sequence ({index})", message));
                }
            }
            else if (!actualMoveNext)
            {
                Assert.Fail(
                    CombineMessages($"Expected sequence exceeded length of actual sequence ({index})", message));
            }
            else
            {
                if (!comparer.Equals(expectedEnumerator.Current, actualEnumerator.Current))
                {
                    Assert.Fail(
                        CombineMessages(
                            $"Expected value at index {index} ({expectedEnumerator.Current}) did not match"
                                + $" actual value ({actualEnumerator.Current})",
                            message));
                }
            }
        }
        while (expectedMoveNext);
    }

    /// <summary>
    /// Formats a required message with an optional message.
    /// </summary>
    /// <param name="requiredMessage"></param>
    /// <param name="optionalMessage"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string CombineMessages(string requiredMessage, string? optionalMessage)
        => string.IsNullOrWhiteSpace(optionalMessage)
            ? $"{requiredMessage}."
            : requiredMessage + $": {optionalMessage}.";
}
