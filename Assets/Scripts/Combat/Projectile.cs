using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 10;
    
    private Health target = null;
    private float damage = 0;
    private void Update()
    {
        if (target == null) return;
        
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);
    }

    

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        else
        {
            target.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
