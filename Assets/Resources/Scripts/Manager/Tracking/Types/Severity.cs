using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.Tracking.Type
{
    public enum SeverityEnum { Infinitive = 100, Critical = 95, Major = 75, Minor = 50, Trivial = 25, PowerUp = 10, PowerDown = -10 };
    public abstract class SeverityTypeAbstract
    {

    }

    public class Severity
    {
        public static Dictionary<SeverityEnum, SeverityTypeAbstract> SeverityTypes { get; } = new Dictionary<SeverityEnum, SeverityTypeAbstract>()
        {
            {SeverityEnum.Critical, new Critical()},
            {SeverityEnum.Major, new Major()},
            {SeverityEnum.Minor, new Minor()},
            {SeverityEnum.Trivial, new Trivial()}
        };

        public sealed class Critical : SeverityTypeAbstract
        {
            //int x = (int)SeverityEnum.Critical;
        }

        public sealed class Major : SeverityTypeAbstract
        {

        }

        public sealed class Minor : SeverityTypeAbstract
        {

        }

        public sealed class Trivial : SeverityTypeAbstract
        {

        }
    }
}
