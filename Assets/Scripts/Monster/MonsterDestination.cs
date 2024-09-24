using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterDestination : MonoBehaviour
    {
        private static List<MonsterDestination> Instances = new List<MonsterDestination>();
    
        public static MonsterDestination GetRandomDestination() => Instances.Count == 0 ? null :  Instances[Random.Range(0, Instances.Count)];
    
        private void OnEnable()
        {
            Instances.Add(this);
        }

        private void OnDisable()
        {
            Instances.Remove(this);
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.05f);
        }
    }
}
