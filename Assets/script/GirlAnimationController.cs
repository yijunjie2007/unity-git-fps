using UnityEngine;

public class GirlAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    // ������������
    private const string SPEED_PARAM = "Speed";
    private const string IS_RUNNING_PARAM = "IsRunning";
    private const string IS_JUMPING_PARAM = "IsJumping";
    private const string GROUNDED_PARAM = "Grounded";

    // �ƶ�����
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found!");
        }
    }

    void Update()
    {
        // ����Ƿ��ڵ���
        isGrounded = characterController.isGrounded;
        animator.SetBool(GROUNDED_PARAM, isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ��΢������ȷ����ɫ�ȶ��ڵ���
        }

        // ��ȡ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // �ж��Ƿ����ܲ�
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        animator.SetBool(IS_RUNNING_PARAM, isRunning);

        // �����ƶ��ٶ�
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveVelocity = moveDirection * currentSpeed;

        // ���ö����ٶȲ��� (0-1��Χ)
        float animationSpeed = moveVelocity.magnitude / runSpeed;
        animator.SetFloat(SPEED_PARAM, animationSpeed);

        // �ƶ���ɫ
        if (moveDirection.magnitude >= 0.1f)
        {
            // ��ת��ɫ�����ƶ�����
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            characterController.Move(moveVelocity * Time.deltaTime);
        }

        // ��Ծ����
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger(IS_JUMPING_PARAM);
        }

        // Ӧ������
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}