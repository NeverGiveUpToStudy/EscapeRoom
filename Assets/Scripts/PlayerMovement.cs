using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Joystick moveStick;//遥控杆组件
    [SerializeField] private float turnSpeed = 20f;//角色移动速度

    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private AudioSource m_AudioSource;


    private void Start() {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate() {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        float horizontal = moveStick.Horizontal;
        float vertical = moveStick.Vertical;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (isWalking) {
            if (!m_AudioSource.isPlaying)
                m_AudioSource.Play();
        } else {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    private void OnAnimatorMove() {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
