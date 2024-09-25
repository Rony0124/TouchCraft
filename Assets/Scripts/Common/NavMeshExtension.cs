using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NavMeshExtension
{
    public static bool TryGetPathRemainingDistance(this UnityEngine.AI.NavMeshAgent navMeshAgent, out float distance)
    {
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0 ||
            float.IsInfinity(navMeshAgent.remainingDistance))
        {
            distance = 0.0f;
            return false;
        }

        distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return true;
    }
}
