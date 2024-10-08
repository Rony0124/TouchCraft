using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Monster
{
    public class MonsterAI : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float patrolSpeed;
        [SerializeField] private float chaseSpeed;
        
        [Header("Attack")]
        [SerializeField] private float attackDuration;
        
        private NavMeshAgent _navMeshAgent;
        private MonsterDestination _targetDestination;
        private MonsterAIState _aiState = MonsterAIState.Idle;
        private MonsterFOV _fov;

        private void Awake()
        {
            _fov = GetComponent<MonsterFOV>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            if (_navMeshAgent)
            {
                _navMeshAgent.enabled = true;
            }

            _fov.onFollowTarget = TargetInSight;
            _fov.onAttack = TargetInAttackArea;
        }

        public void Update()
        {
            switch (_aiState)
            {
                /*case MonsterAIState.Idle:
                    SetNewDestination();
                    break;
                case MonsterAIState.FollowingPath:
                    UpdateNewDestination();
                    break;*/
                
                case MonsterAIState.TargetInSight:
                    if (!_fov.IsTargetExist)
                    {
                        SetNewDestination();
                    }
                    break;
                
                case MonsterAIState.TargetInAttackArea:
                    if (attackDuration >= Time.time)
                    {
                        SetNewDestination();
                    }
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
            _navMeshAgent.speed = patrolSpeed;
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

        private void TargetInSight()
        {
            if(_aiState == MonsterAIState.TargetInAttackArea)
                return;
            
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = chaseSpeed;
            _navMeshAgent.SetDestination(_fov.targetsInSight[0].position);
            
            _aiState = MonsterAIState.TargetInSight;
        }

        private void TargetInAttackArea()
        {
            _aiState = MonsterAIState.TargetInAttackArea;
            
            if (_navMeshAgent.TryGetPathRemainingDistance(out var remainingDistance))
            {
                if (remainingDistance < 1f && attackDuration < Time.time)
                {
                    _navMeshAgent.enabled = false;
                    attackDuration = Time.time + 5;
                    Debug.Log($"Attack the target {_fov.targetsInSight[0].name}");
                }
            }
            else
            {
                SetNewDestination();
            }
            
        }
    }
}
