using UnityEngine;
using System;

public enum JobType
{
    Haul,
    Build,
    Harvest,
    Craft
}

public class Job
{
    public string name = "Default";
    public JobType type;
    public Vector3 location;
    public Action onComplete;
}
