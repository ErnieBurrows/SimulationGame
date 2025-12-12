using UnityEngine;
using UnityEngine.AI;


public class VillagerAIController : SimulatableBehaviour
{
    private NavMeshAgent agent;
    private VillagerNeedsController needs;

    private Job currentJob;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        needs = GetComponent<VillagerNeedsController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        needs.OnThirsty += HandleThirsty;
        needs.OnHungry += HandleHungry;
        needs.OnHydrated += HandleNeedsSatisfied;
        needs.OnFull += HandleNeedsSatisfied;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        needs.OnThirsty -= HandleThirsty;
        needs.OnHungry -= HandleHungry;
        needs.OnHydrated -= HandleNeedsSatisfied;
        needs.OnFull -= HandleNeedsSatisfied;
    }

    public override void Simulate(float dt)
    {
        // Needs override jobs
        if (needs.Environment.IsInWater || needs.Environment.IsInFood)
            return;

        if (currentJob != null)
        {
            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                CompleteJob();
            }
        }
        else
        {
            TryTakeJob();
        }
    }

    private void CompleteJob()
    {
        currentJob.onComplete?.Invoke();
        currentJob = null;
    }

    // ---------------- NEEDS ----------------

    private void HandleThirsty()
    {
        MoveToClosestResource(ResourceType.Water);
        CancelCurrentJob();
    }

    private void HandleHungry()
    {
        MoveToClosestResource(ResourceType.Food);
        CancelCurrentJob();
    }

    private void HandleNeedsSatisfied()
    {
        ClearDestination();
    }

    // ---------------- JOBS ----------------

    private void TryTakeJob()
    {
        currentJob = JobManager.Dequeue();
        if (currentJob == null)
        {
            Wander();
            return;
        }

        agent.SetDestination(currentJob.location);
    }

    private void CancelCurrentJob()
    {
        if (currentJob != null)
        {
            JobManager.Enqueue(currentJob); 
            currentJob = null;
        }
    }

    // ---------------- MOVEMENT ----------------

    private void MoveToClosestResource(ResourceType type)
    {
        Resource best = ResourceRegistry.GetClosest(type, transform.position);
        if (best != null)
            agent.SetDestination(best.transform.position);
    }

    private void ClearDestination()
    {
        if (agent.hasPath)
            agent.ResetPath();
    }

    private void Wander()
    {
        if (agent == null) return;

        for (int i = 0; i < 10; i++)
        {
            Vector3 point = RandomNavmeshLocation(50f);

            if (!IsPointInsideResource(point))
            {
                agent.SetDestination(point);
                return;
            }
        }

        // Fallback: move slightly forward
        agent.SetDestination(transform.position + transform.forward * 2f);
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 random = Random.insideUnitSphere * radius + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(random, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    private bool IsPointInsideResource(Vector3 point)
    {
        Collider[] hits = Physics.OverlapSphere(point, 0.3f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Resource"))
                return true;
        }
        return false;
    }
}
