using System.Runtime.Serialization;

namespace Rnd;

[Serializable]
public class CliException : Exception
{
    public CliException(string s) : base(s)
    {
    }

    protected CliException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
