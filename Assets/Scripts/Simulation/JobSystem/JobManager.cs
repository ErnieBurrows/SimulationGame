using System;
using System.Collections.Generic;
using UnityEngine;

public static class JobManager
{
    public static Queue<Job> jobs = new Queue<Job>();

    public static void Enqueue(Job job)
    {
        jobs.Enqueue(job);
    }

    public static Job Dequeue()
    {
        if (jobs.Count > 0)
        {
            return jobs.Dequeue();
        }
        else
        {
            return null;
        }
    }
}
