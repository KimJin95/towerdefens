using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

// ��ź ���
public class GrabObject : MonoBehaviour
{
    // �ʿ� �Ӽ�: ��ü�� ��� �ִ��� ����, ��� �ִ� ��ü, ���� ��ü�� ����, ���� �� �ִ� �Ÿ�
    // ��ü�� ��� �ִ����� ����

    [SerializeField] LayerMask grabbedLayer;
    [SerializeField] float grabRange;

    bool isGrabbing = false;
    GameObject grabbedObj;

    int closet = 0;
    float minDistance = 9999;


    private void Update()
    {
        if (isGrabbing == false)
        {
            TryGrab();

        }
        else
        {
            TryUnGrab();
        }
    }

    private void TryUnGrab()
    {
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger))
        {
            isGrabbing = false;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObj.transform.parent = null;
            grabbedObj = null;
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger))
        {
            Collider[] hitobjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);

            for (int i = 0; i < hitobjects.Length; i++)
            {
                float dist = Vector3.Distance(hitobjects[i].transform.position, ARAVRInput.RHandPosition);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closet = i;
                }
            }

            if (hitobjects.Length > 0)
            {
                isGrabbing = true;
                grabbedObj = hitobjects[closet].gameObject;
                grabbedObj.transform.parent = ARAVRInput.RHand;
                grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
