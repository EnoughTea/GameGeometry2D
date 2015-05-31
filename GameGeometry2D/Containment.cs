using System;
using System.Runtime.Serialization;

namespace GameGeometry2D {
    [DataContract(Name = "containment", Namespace = ""), Flags]
    public enum Containment {
        Unknown = 0,
        Disjoint = 1 << 0,
        Contains = 1 << 1,
        Intersects = Disjoint | Contains
    }
}