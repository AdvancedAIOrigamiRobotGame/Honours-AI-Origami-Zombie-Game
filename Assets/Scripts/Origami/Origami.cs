using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class Origami : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform RootTransform;
    [SerializeField]
    private float AttackDelay = 0.25f;
    [SerializeField]
    private int Damage = 10;
    [SerializeField]
    private Multiplier BulletPrefab;
    [SerializeField]
    private LayerMask LayerMask;
    [SerializeField]
    private Vector3 BulletSpread = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField]
    private float BulletSpeed = 0.25f;
    [SerializeField]
    private float BulletForce = 100;
    [SerializeField]
    private NavMeshAgent Agent;

    private HashSet<ZombieAttack> Attackables = new HashSet<ZombieAttack>();
    private ZombieAttack CurrentAttackable;
    private Coroutine AttackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        ZombieAttack attackable = other.GetComponentInParent<ZombieAttack>();
        if (attackable != null && attackable.Life > 0)
        {
            if (attackable.OnDie == null)
            {
                attackable.OnDie += HandleDeath;
            }

            Attackables.Add(attackable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ZombieAttack attackable = other.GetComponentInParent<ZombieAttack>();
        if (attackable != null)
        {
            attackable.OnDie = null;
            Attackables.Remove(attackable);
        }
    }

    private void HandleDeath(ZombieAttack Attackable)
    {
        Attackables.Remove(Attackable);
    }

    private void Update()
    {
        if (Attackables.Count > 0)
        {
            Agent.updateRotation = false;
            ZombieAttack closestAttackable = Attackables
                .OrderBy(attackable => Vector3.Distance(transform.position, attackable.transform.position))
                .First();

            Quaternion lookRotation = Quaternion.LookRotation(
                (closestAttackable.transform.position - RootTransform.position).normalized,
                Vector3.up
            );
            RootTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

            if (CurrentAttackable != closestAttackable)
            {
                //closestAttackable.Zombierepresentation
                CurrentAttackable = closestAttackable;
                bool isSelf = NSA.detectNonSelf(CurrentAttackable.zombieR);
                if (!isSelf)
                {
                    if (AttackCoroutine != null)
                    {
                        StopCoroutine(AttackCoroutine);
                    }

                    AttackCoroutine = StartCoroutine(Attack(closestAttackable));
                }
               
            }
        }
        else
        {
            Agent.updateRotation = true;
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
            }
        }
    }

    private IEnumerator Attack(ZombieAttack Attackable)
    {
        while (Attackable != null && Attackable.Life > 0)
        {
            ShootingSystem.Play();
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

            TrailRenderer trail = ObjectMultiplier.CreateInstance(BulletPrefab, 10)
                .GetObject()
                .GetComponent<TrailRenderer>();

            trail.transform.position = ShootingSystem.transform.position;
            trail.Clear();
            Vector3 direction = (Attackable.transform.position - ShootingSystem.transform.position).normalized + new Vector3(
                Random.Range(-BulletSpread.x, BulletSpread.x),
                Random.Range(-BulletSpread.y, BulletSpread.y),
                Random.Range(-BulletSpread.z, BulletSpread.z)
            );
            direction.y = 0;
            direction.Normalize();

            if (Physics.Raycast(ShootingSystem.transform.position,
                direction,
                out RaycastHit hit,
                float.MaxValue,
                LayerMask))
            {
                StartCoroutine(MoveTrail(
                    Attackable,
                    trail,
                    hit.point,
                    hit.collider.GetComponent<Rigidbody>(),
                    true
                ));
            }
            else
            {
                StartCoroutine(MoveTrail(Attackable, trail, direction * 100, null, false));
            }

            yield return Wait;
        }
    }

    private IEnumerator MoveTrail(ZombieAttack Attackable, TrailRenderer Trail, Vector3 HitPoint, Rigidbody HitBody, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        Vector3 direction = (HitPoint - Trail.transform.position).normalized;

        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * BulletSpeed;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            Attackable.TakeDamage(Damage);
            if (HitBody != null)
            {
                HitBody.AddForce(direction * BulletForce, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(Trail.time);

        Trail.gameObject.SetActive(false);
    }
}
