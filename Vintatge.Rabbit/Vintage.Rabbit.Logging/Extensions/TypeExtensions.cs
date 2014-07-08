using System;

namespace Vintage.Rabbit.Logging.Extensions
{
    public static class TypeExtensions
    {
        public static UInt16 ToEventId(this Type type)
        {
            var candidateEventId = type.FullName.GetHashCode();

            candidateEventId = FromHashcodeToValidEventId(candidateEventId);
            return (UInt16) candidateEventId;
        }

        internal static int FromHashcodeToValidEventId(int hashCode)
        {
            const int range = UInt16.MaxValue - 10000;

            hashCode = (hashCode == Int32.MinValue) ? Int32.MaxValue : Math.Abs(hashCode);

            var candidateEventId = hashCode % range;
            candidateEventId += 10000;

            return candidateEventId;
        }
    }
}
