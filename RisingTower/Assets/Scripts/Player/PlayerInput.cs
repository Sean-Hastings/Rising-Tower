using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public enum Inputs
    {
        MouseLeft,
        MouseRight,
        MouseWheel,
        Key1,

    }

    public Camera active_camera;

    protected Dictionary<Inputs, List<Action>> input_handlers = new Dictionary<Inputs, List<Action>>();

    protected bool player_input_blocked;

    protected Target mouse_pos;
    protected bool mouse_left;
    protected bool mouse_right;
    protected float mouse_wheel;
    protected bool key1;

    void Start()
    {
    }

    void Update()
    {
        UpdateMousePos();
        UpdateMouseLeft();
        UpdateMouseRight();
        UpdateMouseWheel();
        UpdateKey1();
    }

    protected void UpdateKey1()
    {
        key1 = Input.GetKey(KeyCode.Alpha1);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FireEvent(Inputs.Key1);
        }
    }

    protected void UpdateMousePos()
    {
        Ray ray = active_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, LayerMask.GetMask("Player"));


        if (hit.transform == null)
        {
            mouse_pos = null;
        }
        else
        {
            Interactable interactable = hit.transform.gameObject.GetComponent<Interactable>();

            if (interactable == null)
            {
                mouse_pos = new Target(hit.point);
            }
            else
            {
                mouse_pos = new Target(interactable);
            }
        }
    }

    protected void UpdateMouseLeft()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            mouse_left = Input.GetMouseButton(0);
            FireEvent(Inputs.MouseLeft);
        }
    }

    protected void UpdateMouseRight()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1))
        {
            mouse_right = Input.GetMouseButton(1);
            FireEvent(Inputs.MouseRight);
        }
    }

    protected void UpdateMouseWheel()
    {
        mouse_wheel = Input.GetAxis("Mouse ScrollWheel");
        FireEvent(Inputs.MouseWheel);
    }

    public void RegisterHandler(Inputs impetus, Action action)
    {
        List<Action> actions = new List<Action>(); ;
        if (!input_handlers.TryGetValue(impetus, out actions))
        {
            actions = new List<Action>();
        }

        actions.Add(action);

        input_handlers[impetus] = actions;
    }

    protected void FireEvent(Inputs impetus)
    {
        List<Action> actions = null;
        if (input_handlers.TryGetValue(impetus, out actions))
        {
            foreach (Action action in actions)
            {
                action();
            }
        }
    }

    public float MouseWheel
    {
        get
        {
            if (player_input_blocked)
            {
                return 0;
            }
            else
            {
                return mouse_wheel;
            }
        }
    }

    public bool MouseLeft
    {
        get
        {
            if (player_input_blocked)
            {
                return false;
            }
            else
            {
                return mouse_left;
            }
        }
    }

    public bool MouseRight
    {
        get
        {
            if (player_input_blocked)
            {
                return false;
            }
            else
            {
                return mouse_right;
            }
        }
    }

    public Target MousePosition
    {
        get
        {
            if (player_input_blocked)
            {
                return null;
            }
            else
            {
                return mouse_pos;
            }
        }
    }
}
