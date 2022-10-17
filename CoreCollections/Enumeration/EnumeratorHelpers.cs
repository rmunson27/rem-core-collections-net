using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.Enumeration;

/// <summary>
/// A series of helper methods for working with enumerators.
/// </summary>
internal static class Enumerators
{
    /// <summary>
    /// Gets an exception that can be thrown appropriately when <see cref="System.Collections.IEnumerator.MoveNext"/>
    /// is called on an enumeration that has not yet started.
    /// </summary>
    public static InvalidOperationException NotStartedException => new("Enumeration not started. Call MoveNext.");

    /// <summary>
    /// Gets an exception that can be thrown appropriately when <see cref="System.Collections.IEnumerator.MoveNext"/>
    /// is called on an enumeration that is already finished.
    /// </summary>
    public static InvalidOperationException AlreadyFinishedException => new("Enumeration already finished.");
}
