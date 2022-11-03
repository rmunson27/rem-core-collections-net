using Rem.Core.Attributes;
using Rem.Core.Collections.Enumeration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Collections;

/// <summary>
/// Represents a dimension of a 2-dimensional collection.
/// </summary>
/// <remarks>
/// The default value of this struct represents a row.
/// </remarks>
public readonly record struct Dimension2D
{
    #region Constants
    /// <summary>
    /// Gets an object that can be used to enumerate through all possible <see cref="Dimension2D"/> values.
    /// </summary>
    public static readonly StaticEnumerable All = StaticEnumerable.Instance;

    /// <summary>
    /// Represents a row of a 2-dimensional list.
    /// </summary>
    public static readonly Dimension2D Row = default;

    /// <summary>
    /// Represents a column of a 2-dimensional list.
    /// </summary>
    public static readonly Dimension2D Column = new(Values.Column);
    #endregion

    /// <summary>
    /// Gets the value of the dimension as an <see langword="enum"/>.
    /// </summary>
    [NameableEnum] public Values Value { get; }

    private Dimension2D(Values Value)
    {
        this.Value = Value;
    }

    /// <summary>
    /// An enumerable that enumerates through all possible <see cref="Dimension2D"/> values.
    /// </summary>
    public sealed class StaticEnumerable : IEnumerable<Dimension2D>
    {
        /// <summary>
        /// An array containing all possible values of the <see cref="Dimension2D"/> struct.
        /// </summary>
        private static readonly Dimension2D[] AllValues
            = Unsafe.As<Values[]>(Enum.GetValues(typeof(Values))).Select(v => new Dimension2D(v)).ToArray();

        /// <summary>
        /// The sole instance of this class.
        /// </summary>
        internal static readonly StaticEnumerable Instance = new();

        private StaticEnumerable() { }

        /// <summary>
        /// Gets an enumerator that enumerates through all possible <see cref="Dimension2D"/> values.
        /// </summary>
        /// <returns></returns>
        public ArrayEnumerator<Dimension2D> GetEnumerator() => new(AllValues);

        IEnumerator IEnumerable.GetEnumerator() => AllValues.GetEnumerator();

        IEnumerator<Dimension2D> IEnumerable<Dimension2D>.GetEnumerator()
            => (AllValues as IEnumerable<Dimension2D>).GetEnumerator();
    }

    /// <summary>
    /// Represents all values of the <see cref="Dimension2D"/> struct.
    /// </summary>
    public enum Values : byte
    {
        /// <summary>
        /// Represents a row.
        /// </summary>
        Row = 0,

        /// <summary>
        /// Represents a column.
        /// </summary>
        Column = 1,
    }

    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Explicitly converts an <see cref="int"/> to a <see cref="Dimension2D"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <exception cref="InvalidCastException">
    /// <paramref name="i"/> is not equivalent to one of the named values of <see cref="Values"/>.
    /// </exception>
    public static explicit operator Dimension2D(int i) => i switch
    {
        (int)Values.Row => Row,
        (int)Values.Column => Column,
        _ => throw new InvalidCastException($"Invalid dimension int value {i}."),
    };

    /// <summary>
    /// Implicitly converts a <see cref="Dimension2D"/> to an <see cref="int"/>.
    /// </summary>
    /// <param name="d"></param>
    public static implicit operator int(Dimension2D d) => (int)d.Value;
}

