using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick moveStick; //遥控杆组件
    [SerializeField] private float turnSpeed = 20f; //角色移动速度
    private Animator m_Animator;
    private AudioSource m_AudioSource;

    private Vector3 m_Movement;
    private Rigidbody m_Rigidbody;
    private Quaternion m_Rotation = Quaternion.identity;


    private void Start()
    {
        moveStick.GetComponent<VariableJoystick>().SetMode(JoystickType.Dynamic);
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        var horizontal = moveStick.Horizontal;
        var vertical = moveStick.Vertical;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        var hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        var hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        var isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        var desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
