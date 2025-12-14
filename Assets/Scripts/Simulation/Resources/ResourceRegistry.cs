using System.Collections.Generic;
using UnityEngine;

public static class ResourceRegistry
{
    private static readonly Dictionary<ResourceType, HashSet<Resource>> resources
        = new Dictionary<ResourceType, HashSet<Resource>>();

    public static void Register(Resource resource)
    {
        if (resource == null) return;

        if (!resources.TryGetValue(resource.resourceType, out var set))
        {
            set = new HashSet<Resource>();
            resources.Add(resource.resourceType, set);
        }

        set.Add(resource);
    }

    public static void Unregister(Resource resource)
    {
        if (resource == null) return;

        if (resources.TryGetValue(resource.resourceType, out var set))
        {
            set.Remove(resource);
        }
    }

    public static Resource GetClosest(ResourceType type, Vector3 position)
    {
        if (!resources.TryGetValue(type, out var set) || set.Count == 0)
            return null;

        Resource closest = null;
        float bestDist = float.MaxValue;

        foreach (var res in set)
        {
            if (res == null || res.currentAmount <= 0) continue;

            float d = Vector3.Distance(position, res.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                closest = res;
            }
        }

        return closest;
    }

    public static IEnumerable<Resource> GetAll(ResourceType type)
    {
        if (resources.TryGetValue(type, out var set))
            return set;

        return System.Array.Empty<Resource>();
    }

    public static void Clear()
    {
        resources.Clear();
    }
}
