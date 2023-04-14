using Rem.Core.Attributes;
using Rem.Core.Collections;
using Rem.Core.Collections.Test.Tools;
using Rem.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests the <see cref="ReadOnlyArray{T}"/> and <see cref="ReadOnlyArray"/> types.
/// </summary>
[TestClass]
public class ReadOnlyArrayTest
{
    /// <summary>
    /// An object that can be used to test sequence equality methods.
    /// </summary>
    private static readonly SequenceEqualityTestMethodsType SequenceEqualityMethods = new();

    /// <summary>
    /// Tests the <see cref="ReadOnlyArray.Clone{T}(T[])"/> and <see cref="ReadOnlyArray{T}.GetClone"/> methods.
    /// </summary>
    [TestMethod]
    public void TestClone()
    {
        int[] arr = new[] { 1, 2, 3 };
        var readOnlyArr = arr.AsReadOnlyArray();

        // Returned array should not equal the original array
        var arrClone = readOnlyArr.GetClone();
        Assert.IsTrue(arr.SequenceEqual(arrClone));
        Assert.AreNotEqual(arr, arrClone);

        // Readonly wrapper for cloned array should not equal readonly wrapper for the original array
        var readOnlyArrClone = ReadOnlyArray.Clone(arr);
        Assert.IsTrue(readOnlyArr.SequenceEqual(readOnlyArrClone));
        Assert.AreNotEqual(readOnlyArr, readOnlyArrClone);
    }

    #region SequenceEqualityMethods Implementations
    /// <summary>
    /// Tests the
    /// <see cref="ReadOnlyArray.SequenceEqual{T}(ReadOnlyArray{T}, IEnumerable{T}, IEqualityComparer{T}?)"/> method.
    /// </summary>
    [TestMethod]
    public void TestSequenceEqual() => SequenceEqualityMethods.RunEqualsTest();

    /// <summary>
    /// Tests the
    /// <see cref="ReadOnlyArray.GetSequenceHashCode{T}(ReadOnlyArray{T}, IEqualityComparer{T}?)"/> method.
    /// </summary>
    [TestMethod]
    public void TestGetSequenceHashCode() => SequenceEqualityMethods.RunGetHashCodeTest();

    /// <summary>
    /// Tests the
    /// <see cref="INestedEqualityComparer{TGeneric, TParameter}.Equals(TGeneric, TGeneric, IEqualityComparer{TParameter})"/>
    /// method of the return value of <see cref="ReadOnlyArray.SequenceEqualityComparer{T}"/>.
    /// </summary>
    [TestMethod]
    public void TestSequenceComparerEqual() => SequenceEqualityMethods.RunComparerEqualsTest();

    /// <summary>
    /// Tests the
    /// <see cref="INestedEqualityComparer{TGeneric, TParameter}.GetHashCode(TGeneric, IEqualityComparer{TParameter})"/>
    /// method of the return value of <see cref="ReadOnlyArray.SequenceEqualityComparer{T}"/>.
    /// </summary>
    [TestMethod]
    public void TestSequenceComparerGetHashCode() => SequenceEqualityMethods.RunComparerGetHashCodeTest();
    #endregion

    #region Helpers
    private sealed class SequenceEqualityTestMethodsType : SequenceEqualityTestHelpers<ReadOnlyArray<float>>
    {
        public SequenceEqualityTestMethodsType() : base(new TestMethodsType()) { }

        /// <inheritdoc/>
        protected override INestedEqualityComparer<ReadOnlyArray<float>, float> Comparer { get; }
            = ReadOnlyArray.SequenceEqualityComparer<float>();

        /// <inheritdoc/>
        protected override int GetSequenceHashCode([DisallowDefault] ReadOnlyArray<float> collection)
            => collection.GetSequenceHashCode();
    }

    private sealed class TestMethodsType : TestHelpers<ReadOnlyArray<float>, float>
    {
        /// <inheritdoc/>
        public override ReadOnlyArray<float> CreateRange(params float[] range) => range.AsReadOnlyArray();

        /// <inheritdoc/>
        public override bool SequenceEqual(
            [DisallowDefault] ReadOnlyArray<float> lhs, [DisallowDefault] ReadOnlyArray<float> rhs)
            => lhs.SequenceEqual(rhs);
    }
    #endregion
}
