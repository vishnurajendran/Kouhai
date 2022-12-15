using MoonSharp.Interpreter;

namespace Kouhai.Scripting.Serialisation
{
    public interface IDVSerialisationContext
    {
        /// <summary>
        /// returns supported type for this serialisation context
        /// </summary>
        DataType SupportedType { get; }
        /// <summary>
        /// Deserialises string to DynValue
        /// </summary>
        /// <param name="serialisedString">the serialised data for the DynValue</param>
        /// <returns></returns>
        DynValue Deserialise(Script script,string serialisedString);
        /// <summary>
        /// Serialises dynvalue to string
        /// </summary>
        /// <param name="dynValue">DynValue to serialise</param>
        /// <returns></returns>
        string Serialise(DynValue dynValue);
    }
}