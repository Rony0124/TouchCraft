using System;
using UnityEngine;
using UnityEngine.AI;

namespace Monster
{
    public class MonsterAI : MonoBehaviour
    {
        [SerializeField] private float monsterSpeed;
        
        private NavMeshAgent _navMeshAgent;
        private MonsterDestination _targetDestination;
        private MonsterAIState _aiState = MonsterAIState.Idle;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            if (_navMeshAgent)
            {
                _navMeshAgent.enabled = true;
            }
        }

        public void Update()
        {
            switch (_aiState)
            {
                case MonsterAIState.Idle:
                    SetNewDestination();
                    break;
                case MonsterAIState.FollowingPath:
                    UpdateNewDestination();
                    break;
            }
        }
        
        private void SetNewDestination()
        {
            _targetDestination = MonsterDestination.GetRandomDestination();

            if (!_targetDestination) 
                return;
                
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = monsterSpeed;
            _navMeshAgent.SetDestination(_targetDestination.transform.position);
            
            _aiState = MonsterAIState.FollowingPath;
        }

        private void UpdateNewDestination()
        {
            if (_navMeshAgent.TryGetPathRemainingDistance(out var remainingDistance))
            {
                if (remainingDistance < 0.1f)
                {
                    SetNewDestination();
                }
            }
            else
            {
                SetNewDestination();
            }
        }
        
    
    }
}
