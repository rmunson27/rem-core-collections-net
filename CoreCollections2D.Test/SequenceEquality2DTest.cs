using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections2D.Test;
using Index2D = ValueTuple<long, long>;

/// <summary>
/// Tests of the <see cref="SequenceEquality2D"/> class functionality.
/// </summary>
[TestClass]
public class SequenceEquality2DTest
{
    #region Constants
    /// <summary>
    /// A 2-dimensional array that is 2-dimensional sequence equivalent to <see cref="Array2"/>.
    /// </summary>
    private static readonly Index2D[,] Array = Values.Indices(2, 3);

    /// <summary>
    /// A 2-dimensional array that is 2-dimensional sequence equivalent to <see cref="Array"/>.
    /// </summary>
    private static readonly Index2D[,] Array2 = Values.Indices(2, 3);

    /// <summary>
    /// A 2-dimensional array that is sequence equivalent to <see cref="Array"/> and <see cref="Array2"/>, but not
    /// 2-dimensional sequence equivalent.
    /// </summary>
    private static readonly Index2D[,] SequenceEquivalent = new[,]
    {
        { (0L, 0L), (0, 1) },
        { (0L, 2), (1, 0) },
        { (1L, 1), (1, 2) },
    };

    /// <summary>
    /// A 2-dimensional array that is 2-dimensional sequence equivalent to <see cref="Array"/> and
    /// <see cref="Array2"/> except for a mismatched element.
    /// </summary>
    private static readonly Index2D[,] MismatchedElement;

    /// <summary>
    /// A 2-dimensional array that contains <see cref="Array"/> and <see cref="Array2"/> but with an extra row.
    /// </summary>
    private static readonly Index2D[,] ExtraRow = Values.Indices(3, 3);

    /// <summary>
    /// A 2-dimensional array that contains <see cref="Array"/> and <see cref="Array2"/> but with an extra column.
    /// </summary>
    private static readonly Index2D[,] ExtraColumn = Values.Indices(2, 4);
    #endregion

    #region Constructor
    static SequenceEquality2DTest()
    {
        MismatchedElement = Values.Indices(2, 3);
        MismatchedElement[1, 2] = (-1, -1);
    }
    #endregion

    #region Tests
    /// <summary>
    /// Tests the <see cref="SequenceEquality2D.Equals"/> method overloads.
    /// </summary>
    [TestMethod]
    public void TestEquals()
    {
        // Array tests
        Assert.IsTrue(SequenceEquality2D.Equals(Array, Array2));
        Assert.IsFalse(SequenceEquality2D.Equals(Array, SequenceEquivalent));
        Assert.IsFalse(SequenceEquality2D.Equals(Array, MismatchedElement));
        Assert.IsFalse(SequenceEquality2D.Equals(Array, ExtraRow));
        Assert.IsFalse(SequenceEquality2D.Equals(Array, ExtraColumn));

        // Readonly array tests
        ReadOnly2DArray<Index2D> rArray = new(Array), rArray2 = new(Array2),
                                 rSequenceEquivalent = new(SequenceEquivalent),
                                 rMismatchedElement = new(MismatchedElement),
                                 rExtraRow = new(ExtraRow), rExtraColumn = new(ExtraColumn);
        Assert.IsTrue(SequenceEquality2D.Equals(rArray, rArray2));
        Assert.IsFalse(SequenceEquality2D.Equals(rArray, rSequenceEquivalent));
        Assert.IsFalse(SequenceEquality2D.Equals(rArray, rMismatchedElement));
        Assert.IsFalse(SequenceEquality2D.Equals(rArray, rExtraRow));
        Assert.IsFalse(SequenceEquality2D.Equals(rArray, rExtraColumn));
    }

    /// <summary>
    /// Tests the <see cref="SequenceEquality2D.GetHashCode"/> method overloads.
    /// </summary>
    [TestMethod]
    public void TestGetHashCode()
    {
        int arrayHashCode = SequenceEquality2D.GetHashCode(Array),
            array2HashCode = SequenceEquality2D.GetHashCode(Array2),
            sequenceEquivalentHashCode = SequenceEquality2D.GetHashCode(SequenceEquivalent),
            mismatchedElementHashCode = SequenceEquality2D.GetHashCode(MismatchedElement),
            extraRowHashCode = SequenceEquality2D.GetHashCode(ExtraRow),
            extraColumnHashCode = SequenceEquality2D.GetHashCode(ExtraColumn);

        // Array tests
        Assert.AreEqual(arrayHashCode, array2HashCode);
        Assert.AreNotEqual(arrayHashCode, sequenceEquivalentHashCode);
        Assert.AreNotEqual(arrayHashCode, mismatchedElementHashCode);
        Assert.AreNotEqual(arrayHashCode, extraRowHashCode);
        Assert.AreNotEqual(arrayHashCode, extraColumnHashCode);

        // Ensure hash codes match - if so, equality tests from above also hold
        ReadOnly2DArray<Index2D> rArray = new(Array), rArray2 = new(Array2),
                                 rSequenceEquivalent = new(SequenceEquivalent),
                                 rMismatchedElement = new(MismatchedElement),
                                 rExtraRow = new(ExtraRow), rExtraColumn = new(ExtraColumn);
        Assert.AreEqual(arrayHashCode, SequenceEquality2D.GetHashCode(rArray));
        Assert.AreEqual(array2HashCode, SequenceEquality2D.GetHashCode(rArray2));
        Assert.AreEqual(sequenceEquivalentHashCode, SequenceEquality2D.GetHashCode(rSequenceEquivalent));
        Assert.AreEqual(mismatchedElementHashCode, SequenceEquality2D.GetHashCode(rMismatchedElement));
        Assert.AreEqual(extraRowHashCode, SequenceEquality2D.GetHashCode(rExtraRow));
        Assert.AreEqual(extraColumnHashCode, SequenceEquality2D.GetHashCode(rExtraColumn));
    }
    #endregion
}
