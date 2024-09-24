using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Monster
{
    public class MonsterFOV : MonoBehaviour
    {
        [Tooltip("시야 영역의 반지름과 시야 각도")]
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
   
        //타겟 layer, 장애물 layer
        [SerializeField] private LayerMask targetMask, obstacleMask;
    
        public List<Transform> targetsInSight = new ();
    
        public bool IsTargetExist => targetsInSight.Count != 0;

        private void FixedUpdate()
        {
            FindVisibleTargets();
        }

        private void FindVisibleTargets()
        {
            targetsInSight.Clear();
        
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            const int maxColliders = 10;
            var hitColliders = new Collider[maxColliders];
        
            var size = Physics.OverlapSphereNonAlloc(transform.position, viewRadius, hitColliders, targetMask);
            foreach (var hit in hitColliders)
            {
                if(hit == null)
                    continue;
            
                var target = hit.transform;
                var targetPos = target.position;
                var position = transform.position;
                targetPos.y = position.y;
            
                var dirToTarget = (targetPos - position).normalized;
                // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    var dstToTarget = Vector3.Distance(transform.position, targetPos);
               
                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                        continue;
                
                    if (dstToTarget > 0.2f)
                    {
                        targetsInSight.Add(target);
                    }
                }
            }
        }
        
        // y축 오일러 각을 3차원 방향 벡터로 변환한다.
        // 원본과 구현이 살짝 다름에 주의. 결과는 같다.
        public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
        }
    }

    #region Editor

    #if UNITY_EDITOR
    [CustomEditor (typeof (MonsterFOV))]
    public class MonsterFOVEditor : Editor
    {
        void OnSceneGUI()
        {
            var fow = target as MonsterFOV;
            if (fow != null)
            {
                var position = fow.transform.position;
            
                Handles.color = Color.white;
                Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
            
                Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
                Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

                Handles.DrawLine(position, position + viewAngleA * fow.viewRadius);
                Handles.DrawLine(position, position + viewAngleB * fow.viewRadius);
                
                Handles.color = Color.red;
                foreach (var visible in fow.targetsInSight)
                {
                    Handles.DrawLine(fow.transform.position, visible.transform.position);
                }
            }
        }
    } 
    #endif

    #endregion
  
}
