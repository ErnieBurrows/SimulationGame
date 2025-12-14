using UnityEngine;

public class WaterPumpJobSource : JobSource
{
    [Header("Production")]
    [SerializeField] private Resource waterResource;
    [SerializeField] private float waterPerJob = 5f;

    [Header("Timing (Simulation Time)")]
    [SerializeField] private float secondsPerJob = 5f;

    [SerializeField] private float simTimer = 0f;
    private bool jobPending = false;

    public override void Simulate(float dt)
    {
        // Do not produce if full
        if (waterResource.currentAmount >= waterResource.maxAmount)
            return;

        // Do not create multiple jobs at once
        if (jobPending)
            return;

        simTimer += dt;

        if (simTimer >= secondsPerJob)
        {
            CreatePumpJob();
            simTimer = 0f;
            jobPending = true;
        }
    }

    private void CreatePumpJob()
    {
        JobManager.Enqueue(new Job
        {
            type = JobType.Craft,
            location = transform.position,
            onComplete = OnPumpJobComplete
        });
    }

     private void OnPumpJobComplete()
    {
        waterResource.Add(waterPerJob);
        jobPending = false;
    }
}
