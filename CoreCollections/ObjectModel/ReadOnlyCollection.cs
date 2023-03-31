using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections.ObjectModel;

/// <summary>
/// Extends the <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> class and adds standard collection
/// interfaces provided by this library.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReadOnlyCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IReadOnlyContainer<T>
{
    /// <summary>
    /// Constructs a new instance of the <see cref="ReadOnlyCollection{T}"/> class wrapping the specified
    /// <see cref="IList{T}"/>.
    /// </summary>
    /// <param name="list">The list to wrap in the new instance.</param>
    public ReadOnlyCollection(IList<T> list) : base(list) { }
}
