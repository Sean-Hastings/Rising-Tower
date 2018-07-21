using UnityEngine;

public class ProjectileSkill : Skill
{
    public Projectile projectile;

    protected override void ActivateSkill(Actor actor, Target target)
    {
        Vector3 look_position = target.Position;
        look_position.y = actor.transform.position.y;
        target.Position = look_position;
        actor.transform.LookAt(look_position);
        projectile.Spawn(actor).Project(target);
    }
}
