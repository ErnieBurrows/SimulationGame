using UnityEngine;
using System;

public abstract class JobSource : MonoBehaviour
{
    protected void CreateJob(Job job)
    {
        JobManager.Enqueue(job);
    }
}
