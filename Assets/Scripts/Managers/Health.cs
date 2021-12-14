using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float health;
    protected bool isDeath;

    void Start()
    {
        health = maxHealth;
    }

    public virtual void DoDamage(float _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            OnDeath();
            health = 0;
            isDeath = true;
        }
        else
        {
            OnDamage();
        }
    }
    public virtual void AddLife(float _life)
    {
        health += _life;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        OnAddLife();
    }

    public virtual void InstantDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDamage() { }
    protected virtual void OnDeath() { }
    protected virtual void OnAddLife() { }
    public float GetHealth() { return health; }
    public bool IsDeath() { return isDeath; }
}
