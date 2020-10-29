
using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 5.66f;

        private NavMeshAgent navMestAgent;
        private Health health;

        private void Start()
        {
            navMestAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMestAgent.enabled = !health.IsDead();
            
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
      
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMestAgent.destination = destination;
            navMestAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMestAgent.isStopped = false;
        }
        
        public void Cancel()
        {
            navMestAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
        
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }

}