using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Contains static internal methods for dealing with collections.
/// </summary>
internal static class CollectionHelpers
{
    /// <summary>
    /// Gets a new exception that can be thrown when a mutation attempt is made on a readonly collection.
    /// </summary>
    public static NotSupportedException ReadOnlyMutationAttempted => new("Collection is readonly.");
}
