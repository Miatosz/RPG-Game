using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float timeBeetwenAtacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;


        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (target == null) return;
            
            if (target.IsDead()) return;
            
            if (!GetIsInRange())
            {
                if (GetComponent<Transform>().tag == "Enemy")
                {
                    GetComponent<Mover>().MoveTo(target.transform.position, 0.9f);

                }
                else
                {
                    GetComponent<Mover>().MoveTo(target.transform.position, 1f);
                }
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBeetwenAtacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
            
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetWeaponDamage());
            }
        }

        void Shoot()
        {
            Hit();
        }

        bool isPlayer()
        {
            if (GetComponent<Transform>().tag == "Player")
            {
                return true;
            }
            return false;
        }

        public bool CanAttack(GameObject combatTarget)
        {
             if (isPlayer() && combatTarget == GameObject.FindWithTag("Player")) return false;
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
        }

        public void Attack(GameObject combatTarget)
        {
            
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}