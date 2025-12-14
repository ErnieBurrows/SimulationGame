using UnityEngine;

public class Building : JobSource
{
     private void Start()
    {
        CreateInitialJobs();
    }

    private void CreateInitialJobs()
    {
        JobManager.Enqueue(new Job
        {
            type = JobType.Build,
            location = transform.position,
            onComplete = OnBuilt
        });
    }

    private void OnBuilt()
    {
        transform.localScale *= 2.0f;
        Debug.Log("Building completed!");
    }
}
