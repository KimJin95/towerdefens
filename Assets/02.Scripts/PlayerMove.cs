using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자의 입력에 따라 4방으로 이동처리
//중력 적용
//점프(오른쪽 패드 클릭)
//카메라가 바라보는 방향으로 이동

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
