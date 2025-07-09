using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    [Header("�ƶ�����")]
    [SerializeField] private float walkSpeed = 5f;      // �����ٶ�
    [SerializeField] private float runSpeed = 8f;      // �ܲ��ٶ�
    [SerializeField] private float jumpHeight = 1.5f;   // ��Ծ�߶�
    [SerializeField] private float gravity = -9.81f;    // �������ٶ�

    [Header("������")]
    [SerializeField] private Transform groundCheck;     // �������
    [SerializeField] private float groundDistance = 0.1f; // ���������

    [SerializeField] private LayerMask groundMask;      // �����

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // ������
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // ���������ٶȵ��Ӵ�����ʱ
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ��ȡ����
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �ܲ��л�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // �����ƶ����� (�������ҳ���)
        Vector3 move = transform.right * x + transform.forward * z;

        // Ӧ���ƶ�
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ��Ծ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Ӧ������
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}