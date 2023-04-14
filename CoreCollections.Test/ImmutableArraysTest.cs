using Rem.Core.Attributes;
using Rem.Core.Collections.Test.Tools;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests of the <see cref="ImmutableArrays"/> class.
/// </summary>
[TestClass]
public class ImmutableArraysTest
{
    #region Constants
    /// <summary>
    /// Helper methods for running test on <see cref="ImmutableArray{T}"/> instances.
    /// </summary>
    private static readonly TestMethodsType Helpers = new();

    /// <summary>
    /// Helper methods for running tests of the <see cref="ImmutableArrays"/> sequence equality methods.
    /// </summary>
    private static readonly SequenceEqualityTestMethodsType SequenceEqualityHelpers = new(Helpers);
    #endregion

    #region Tests
    /// <summary>
    /// Tests the
    /// <see cref="ImmutableArrays.SequenceEqual{TParent, TChild}(ImmutableArray{TParent}, ReadOnlyArray{TChild}, IEqualityComparer{TParent}?)"/>
    /// method.
    /// </summary>
    [TestMethod]
    public void TestSequenceEqual()
    {
        var arr = new[] { 1, 2, 3 };
        var immutableArr = arr.ToImmutableArray();
        var readOnlyArr = arr.AsReadOnlyArray();

        Assert.IsTrue(immutableArr.SequenceEqual(readOnlyArr));
        Assert.IsFalse(immutableArr.SequenceEqual(new[] { 1, 2, 54 }.AsReadOnlyArray()));
    }

    #region SequenceEqualityMethods Implementations
    /// <summary>
    /// Tests the
    /// <see cref="ImmutableArrays.GetSequenceHashCode{T}(ImmutableArray{T}, IEqualityComparer{T}?)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetSequenceHashCode() => SequenceEqualityHelpers.RunGetHashCodeTest();

    /// <summary>
    /// Tests the
    /// <see cref="INestedEqualityComparer{TGeneric, TParameter}.Equals(TGeneric, TGeneric, IEqualityComparer{TParameter})"/>
    /// method of the <see cref="ImmutableArrays.SequenceEqualityComparer{T}"/> method return value.
    /// </summary>
    [TestMethod]
    public void TestSequenceComparerEquals() => SequenceEqualityHelpers.RunComparerEqualsTest();

    /// <summary>
    /// Tests the
    /// <see cref="INestedEqualityComparer{TGeneric, TParameter}.GetHashCode(TGeneric, IEqualityComparer{TParameter})"/>
    /// ,ethod of the <see cref="ImmutableArrays.SequenceEqualityComparer{T}"/> method return value.
    /// </summary>
    [TestMethod]
    public void TestSequenceComparerGetHashCode() => SequenceEqualityHelpers.RunComparerGetHashCodeTest();
    #endregion
    #endregion

    #region Helpers
    private sealed class SequenceEqualityTestMethodsType : SequenceEqualityTestHelpers<ImmutableArray<float>>
    {
        public SequenceEqualityTestMethodsType(TestMethodsType testMethods) : base(testMethods) { }

        /// <inheritdoc/>
        protected override INestedEqualityComparer<ImmutableArray<float>, float> Comparer { get; }
            = ImmutableArrays.SequenceEqualityComparer<float>();

        /// <inheritdoc/>
        protected override int GetSequenceHashCode([DisallowDefault] ImmutableArray<float> collection)
            => collection.GetSequenceHashCode();
    }

    private sealed class TestMethodsType : TestHelpers<ImmutableArray<float>, float>
    {
        /// <inheritdoc/>
        public override ImmutableArray<float> CreateRange(params float[] range) => ImmutableArray.CreateRange(range);

        /// <inheritdoc/>
        public override bool SequenceEqual(
            [DisallowDefault] ImmutableArray<float> lhs, [DisallowDefault] ImmutableArray<float> rhs)
            => lhs.SequenceEqual(rhs);
    }
    #endregion
}
