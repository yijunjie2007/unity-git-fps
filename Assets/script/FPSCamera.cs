using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("�ӽ�����")]
    [SerializeField] private float mouseSensitivity = 100f; // ���������
    [SerializeField] private Transform playerBody;         // �������(����ˮƽ��ת)

    [Header("�ӽ�����")]
    [SerializeField] private float minVerticalAngle = -90f; // ��С����
    [SerializeField] private float maxVerticalAngle = 90f;  // �������

    private float xRotation = 0f; // ��ǰ��ֱ��ת�Ƕ�

    private void Start()
    {
        // ���������ع��
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ��ֱ�ӽ���ת (���¿�)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        // Ӧ�ô�ֱ��ת (ֻ��ת���)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ˮƽ�ӽ���ת (���ҿ� - ��ת������Ҷ���)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}