using UnityEngine;

public class WaterPumpJobSource : JobSource
{
    [Header("Production")]
    [SerializeField] private Resource waterResource;
    [SerializeField] private float maxHeldWater = 5f;
    [SerializeField] private float waterPumpedPerTick = 0.02f;
    [SerializeField] private float currentHeldWater = 0f;

    private bool jobPending = false;

    public override void Simulate(float dt)
    {
        // Do not produce if full
        if (waterResource.currentAmount >= waterResource.maxAmount)
            return;

        // Do not create multiple jobs at once
        if (jobPending)
            return;

        currentHeldWater += waterPumpedPerTick;

        if (currentHeldWater >= maxHeldWater)
        {
            CreatePumpJob();
            currentHeldWater = 0f;
            jobPending = true;
        }
    }

    private void CreatePumpJob()
    {
        JobManager.Enqueue(new Job
        {
            name = "Picking up water from pump",
            type = JobType.Craft,
            location = transform.position,
            onComplete = OnPumpJobComplete
            
        });
    }

    private void OnPumpJobComplete()
    {
        transform.localScale *= 2.0f;
        waterResource.Add(maxHeldWater);
        jobPending = false;
    }
}
