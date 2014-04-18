using UnityEngine;
using System.Collections;

public enum Layers
{
    ////////////////
    /// Warning! ///
    ////////////////
    /// Do not change existing names and numbers through refractoring only!
    /// These definitions are associated with the layers in Unity.
    /// Changes to existing values must be changed in both places.    

    Team1Actor = 8,
    Team1Mothership = 9,
    Team1Projectile = 10,
    Team2Actor = 11,
    Team2Mothership = 12,
    Team2Projectile = 13,
    Obstacles = 14,
    Scenery = 15,
    Boundary = 16,

    //public const string Team1Actor      = "Team1Actor";
    //public const string Team1Mothership = "Team1Mothership";
    //public const string Team1Projectile = "Team1Projectile";
    //public const string Team2Actor      = "Team2Actor";
    //public const string Team2Mothership = "Team2Mothership";
    //public const string Team2Projectile = "Team2Projectile";
    //public const string Obstacles       = "Obstacles";
    //public const string Scenery         = "Scenery";
    //public const string Boundary        = "Boundary";
}
