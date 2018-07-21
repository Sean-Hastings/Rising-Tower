using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class Actor : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float m_MovingTurnSpeed = 360;
    [SerializeField]
    protected float m_StationaryTurnSpeed = 180;
    [SerializeField]
    protected float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    protected float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    protected float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    protected float m_GroundCheckDistance = 0.1f;

    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;
    protected const float k_Half = 0.5f;
    protected float m_TurnAmount;
    protected float m_ForwardAmount;
    protected Vector3 m_GroundNormal;
    protected float m_CapsuleHeight;
    protected Vector3 m_CapsuleCenter;
    protected CapsuleCollider m_Capsule;


    [SerializeField]
    protected bool friendly;
    [SerializeField]
    protected float max_health;
    [SerializeField]
    protected float cur_heath;
    [SerializeField]
    protected float max_mana;
    [SerializeField]
    protected float cur_mana;


    [SerializeField]
    protected Skill[] skills;

    public Skill GetSkill(int index)
    {
        if (skills.Length > index)
        {
            return skills[index];
        }
        else
        {
            return null;
        }
    }

    public bool UseSkill(int index, Target target)
    {
        return skills.Length > index && skills[index].Act(this, target);
    }

    public bool Friendly
    {
        get
        {
            return friendly;
        }

        set
        {
            friendly = value;
        }
    }

    public float HealthCurrent()
    {
        return cur_heath;
    }

    public bool SpendMana(float mana_cost)
    {
        if (cur_mana >= mana_cost)
        {
            cur_mana -= mana_cost;
            return true;
        }
        else
        {
            return false;
        }
    }


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }


    public void Move(Vector3 move, bool crouch, bool jump)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move.Scale(new Vector3(m_MoveSpeedMultiplier, m_MoveSpeedMultiplier, m_MoveSpeedMultiplier));
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }


    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

        m_Animator.speed = m_AnimSpeedMultiplier;
    }
   

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (Time.deltaTime > 0)
        {
            Vector3 v = m_Animator.deltaPosition / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_Animator.applyRootMotion = true;
        }
        else
        {
            Debug.Log("Airborne");
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }
}