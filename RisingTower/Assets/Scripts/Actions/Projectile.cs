using UnityEngine;

public class Projectile : Spawnable<Projectile>
{
    public float move_speed;
    public float max_distance;

    protected Target target;
    protected Vector3 angle_offset;

    public void Project(Target target)
    {
        Vector3 trajectory = target.Position - transform.position;
        trajectory.Normalize();
        trajectory.Scale(new Vector3(max_distance, max_distance, max_distance));
        target.Position = transform.position + trajectory;

        this.target = target;

        angle_offset = transform.eulerAngles;
        transform.LookAt(target.Position);
        angle_offset -= transform.eulerAngles;
    }

    void Start()
    {
        transform.LookAt(target.Position);
        transform.Rotate(angle_offset);
    }

    void Update()
    {
        float move_dist = move_speed * Time.deltaTime;

        if ((transform.position - target.Position).magnitude < move_dist)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.LookAt(target.Position);
            transform.position += transform.forward * move_dist;
            transform.Rotate(angle_offset);
        }
    }
}