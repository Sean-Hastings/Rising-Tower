using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target {

    public Target(Vector3 position)
    {
        this.position = position;

        HasInteractable = false;
        Interactable = null;
    }

    public Target(Vector3 position, Interactable interactable)
    {
        this.position = position;

        has_interactable = interactable != null;
        this.interactable = interactable;
    }

    public Target(Interactable interactable)
    {
        position = interactable.transform.position;

        has_interactable = interactable != null;
        this.interactable = interactable;
    }

    protected Vector3 position;

    protected bool has_interactable;
    protected Interactable interactable;

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public bool HasInteractable
    {
        get
        {
            return has_interactable;
        }

        set
        {
            has_interactable = value;
        }
    }

    public Interactable Interactable
    {
        get
        {
            return interactable;
        }

        set
        {
            interactable = value;
        }
    }
}
