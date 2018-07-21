using UnityEngine;

public abstract class Spawnable<T> : MonoBehaviour
{
    public bool friendly;

    public T Spawn(Actor actor)
    {
        Transform actor_transform = actor.transform;

        GameObject spawned_object = Instantiate(gameObject, actor_transform.position + actor_transform.forward, actor_transform.rotation);
        spawned_object.transform.Rotate(gameObject.transform.eulerAngles);
        spawned_object.GetComponent<Spawnable<T>>().friendly = actor.Friendly;
        return spawned_object.GetComponent<T>();
    }
}