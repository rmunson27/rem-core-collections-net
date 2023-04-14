using Rem.Core.Collections.Test.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Test;

/// <summary>
/// Tests the LINQ methods of the <see cref="ReadOnlyArray{T}"/> struct.
/// </summary>
[TestClass]
public class ReadOnlyArrayLinqTest : LinqImplementationTest<ReadOnlyArray<Number1>, ReadOnlyArraySlice<Number1>,
                                                            ReadOnlyArray<Number1Child>,
                                                            ReadOnlyArray<Number2>,
                                                            ReadOnlyArray<Number2Child>,
                                                            ReadOnlyArray<Number3>,
                                                            ReadOnlyArray<decimal>, ReadOnlyArray<decimal?>,
                                                            ReadOnlyArray<double>, ReadOnlyArray<double?>,
                                                            ReadOnlyArray<float>, ReadOnlyArray<float?>,
                                                            ReadOnlyArray<int>, ReadOnlyArray<int?>,
                                                            ReadOnlyArray<long>, ReadOnlyArray<long?>>
{
    protected override ReadOnlyArrayLinqImplementation<Number1, Number1Child, Number2, Number2Child, Number3, BigInteger> Implementation { get; }
        = new ReadOnlyArrayLinqImplementation<Number1, Number1Child, Number2, Number2Child, Number3, BigInteger>();

    protected override ReadOnlyArray<Number1> NewNumber1s(IEnumerable<Number1Child> elements)
        => elements.Cast<Number1>().ToArray().AsReadOnlyArray();

    protected override ReadOnlyArraySlice<Number1> NewChunk(IEnumerable<Number1Child> elements)
        => elements.Cast<Number1>().ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<Number1Child> NewNumber1Children(IEnumerable<Number1Child> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<Number2> NewNumber2s(IEnumerable<Number2Child> elements)
        => elements.Cast<Number2>().ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<Number2Child> NewNumber2Children(IEnumerable<Number2Child> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<Number3> NewNumber3s(IEnumerable<Number3> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<decimal> New(IEnumerable<decimal> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<double> New(IEnumerable<double> elements) => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<float> New(IEnumerable<float> elements) => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<int> New(IEnumerable<int> elements) => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<long> New(IEnumerable<long> elements) => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<decimal?> NewNullable(IEnumerable<decimal?> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<double?> NewNullable(IEnumerable<double?> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<float?> NewNullable(IEnumerable<float?> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<int?> NewNullable(IEnumerable<int?> elements)
        => elements.ToArray().AsReadOnlyArray();

    protected override ReadOnlyArray<long?> NewNullable(IEnumerable<long?> elements)
        => elements.ToArray().AsReadOnlyArray();
}
