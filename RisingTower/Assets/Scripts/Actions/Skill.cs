using UnityEngine;

public abstract class Skill : MonoBehaviour, IAction
{
    public float cooldown_max;
    public float cooldown_remaining;

    public float mana_cost;
    public float damage;

    public bool Act(Actor actor, Target target)
    {
        if (cooldown_remaining <= 0 && actor.SpendMana(mana_cost))
        {
            cooldown_remaining = cooldown_max;
            ActivateSkill(actor, target);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected abstract void ActivateSkill(Actor actor, Target target);

    void FixedUpdate()
    {
        if (cooldown_remaining > 0)
        {
            cooldown_remaining -= Time.fixedDeltaTime;
        }
    }
}
