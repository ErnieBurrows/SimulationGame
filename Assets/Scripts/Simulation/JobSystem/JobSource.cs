using UnityEngine;
using System;

public abstract class JobSource : SimulatableBehaviour
{
    protected void CreateJob(Job job)
    {
        JobManager.Enqueue(job);
    }
}
