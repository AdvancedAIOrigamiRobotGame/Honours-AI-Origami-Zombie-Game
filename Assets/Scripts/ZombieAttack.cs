using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [field: SerializeField]
    public int Life
    {
        get; private set;
    } = 100;
    [SerializeField]
    private Ragdoll RagdollEnabler;
    [SerializeField]
    private float FadeOutDelay = 10f;
    [SerializeField]
    public string zombieR;

    public delegate void DeathEvent(ZombieAttack Attackable);
    public DeathEvent OnDie;

    public delegate void TakeDamageEvent();
    public TakeDamageEvent OnTakeDamage;

    private void Start()
    {
        zombieR = scheme.createRepresentation(1);
        if (RagdollEnabler != null)
        {
            RagdollEnabler.EnableAnimator();
        }
    }

    public void TakeDamage(int Damage)
    {
        Life -= Damage;
        OnTakeDamage?.Invoke();
        if (Life <= 0 && RagdollEnabler != null)
        {
            OnDie?.Invoke(this);
            RagdollEnabler.EnableRagdoll();
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(FadeOutDelay);

        if (RagdollEnabler != null)
        {
            RagdollEnabler.DisableAllRigidbodies();
        }

        float time = 0;
        while (time < 1)
        {
            transform.position += Vector3.down * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
