using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������� �Է¿� ���� 4������ �̵�ó��
//�߷� ����
//����(������ �е� Ŭ��)
//ī�޶� �ٶ󺸴� �������� �̵�

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float jumpPower = 5;
    [SerializeField] float gravity = -20f;

    float velocityY;

    CharacterController mycontroller;

    private void Start()
    {
        mycontroller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        velocityY += gravity * Time.deltaTime;
        if (mycontroller.isGrounded) velocityY = 0;

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two)) velocityY = jumpPower;
        dir.y = velocityY;

        mycontroller.Move(dir * speed * Time.deltaTime);
    }
}
