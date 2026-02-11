using System;
using UnityEngine;
using UnityEngine.AI;

// Todo: Change to a system of speed where there is an action that is called when the speed is changed.
// Todo: Put this action in SimulatableBehaviour.
// Todo: Make sure animationSpeed scales with it as well.
public class VillagerAIController : SimulatableBehaviour
{
    private NavMeshAgent agent;
    private VillagerNeedsController needs;
    private Job currentJob;
    private float baseSpeed;
    private bool isWandering = false;
    private bool isGettingFoodOrDrink = false;
    public Action<string> OnJobChanged;

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        needs = GetComponent<VillagerNeedsController>();

        baseSpeed = agent.speed;
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

    #region Main Simulation
    public override void Simulate(float dt)
    {
        if (isGettingFoodOrDrink) return;

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
            if (!TryTakeJob())
                Wander();
        }
    }

    public override void HandleSimulationSpeedChange()
    {
        base.HandleSimulationSpeedChange();

        SimulationManager sim = SimulationManager.Instance;
        if (sim == null) return;
        
        agent.speed = baseSpeed * sim.GetMovementMultiplier();
        agent.isStopped = sim.simulationSpeed == SimulationSpeed.Paused;
    }   
    #endregion

    #region Needs
    private void HandleThirsty()
    {
        if(!TryMoveToClosestResource(ResourceType.WaterDrinkable)) return;
        
        isGettingFoodOrDrink = true;

        if(!TryCancelCurrentJob())
            OnJobChanged?.Invoke(DebugCurrentStateText());
        
    }

    private void HandleHungry()
    {
        if(!TryMoveToClosestResource(ResourceType.Food)) return;
        
        isGettingFoodOrDrink = true;

        if(!TryCancelCurrentJob())
            OnJobChanged?.Invoke(DebugCurrentStateText()); 
        
    }

    private void HandleNeedsSatisfied()
    {
        isGettingFoodOrDrink = false;
        ClearDestination();

        OnJobChanged?.Invoke(DebugCurrentStateText());
    }

    #endregion

    #region Jobs
    private bool TryTakeJob()
    {
        currentJob = JobManager.Dequeue();
        
        if(currentJob != null)
        {
            agent.SetDestination(currentJob.location);
            OnJobChanged?.Invoke(DebugCurrentStateText());
        }

        return currentJob != null;      
    }

    private void CompleteJob()
    {
        currentJob.onComplete?.Invoke();
        currentJob = null;

        OnJobChanged?.Invoke(DebugCurrentStateText());

    }

    private bool TryCancelCurrentJob()
    {
        if (currentJob == null) return false;
      
        JobManager.Enqueue(currentJob);
        currentJob = null;

        OnJobChanged?.Invoke(DebugCurrentStateText()); 
        

        return true;
    }

    public string DebugCurrentStateText()
    {
        if (isGettingFoodOrDrink) return "Getting Food Or Drink";

        if (currentJob != null) return $"Working: {currentJob.name}";

        return "Idle";
    }

    #endregion

    #region Movement

    private bool TryMoveToClosestResource(ResourceType type)
    {
        Resource best = ResourceRegistry.GetClosest(type, transform.position);
        if (best != null)
        {
            agent.SetDestination(best.transform.position);
            return true;
        }
        else
            return false;
    }

    private void ClearDestination()
    {
        if (agent.hasPath)
            agent.ResetPath();
    }

    /// <summary>
    /// Handles the agents current wandering state. If the agent is wandering it will check the distance left.
    /// Otherwise it will find a position that is not within a resource.
    /// </summary>
    private void Wander()
    {
        
        if (agent == null) return;

        if (isWandering)
        {
            if (agent.remainingDistance > 0.1) return;
            
                isWandering = false;
        }
        
        for (int i = 0; i < 10; i++)
        {
            Vector3 point = RandomNavmeshLocation(50f);

            if (!IsPointInsideResource(point))
            {
                agent.SetDestination(point);
                isWandering = true;
                return;
            }
        }

        // Fallback: move slightly forward
        agent.SetDestination(transform.position + transform.forward * 2f);
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 random = UnityEngine.Random.insideUnitSphere * radius + transform.position;
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
    #endregion
}
