using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.25f;

        
        private Fighter fighter;
        private GameObject player;
        private Health healt;
        private Mover mover;
        
        private Vector3 startPosition;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;
        
        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            healt = GetComponent<Health>();
            startPosition = transform.position;
        }

        private void Update()
        {                
            if (CheckIfDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //suspicion state
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
                //fighter.Cancel();
            }

            timeSinceLastSawPlayer += Time.deltaTime;

        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = startPosition;
            
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }
            
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancenCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        bool CheckIfDead()
        {
            return healt.IsDead();
        }
        
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // called by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

