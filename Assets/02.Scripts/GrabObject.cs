using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

// 폭탄 잡기
public class GrabObject : MonoBehaviour
{
    // 필요 속성: 물체를 잡고 있는지 여부, 잡고 있는 물체, 잡을 물체의 종류, 잡을 수 있는 거리
    // 물체를 잡고 있는지의 여부

    [SerializeField] LayerMask grabbedLayer;
    [SerializeField] float grabRange;

    bool isGrabbing = false;
    GameObject grabbedObj;

    int closet = 0;
    float minDistance = 9999;

    Vector3 prevPos; //이전위치
    float throwPower = 10;

    Quaternion prevRot;
    float rotPower = 5;

    [SerializeField] bool isRemoteGrab=true;
    float remoteGrabDist = 20;

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

        Vector3 throwDir = ARAVRInput.RHandPosition - prevPos;
        prevPos = ARAVRInput.RHandPosition;

        //Quaternion의 뺄샘
        //angle1+angle2 = Quaternion * Quaternion
        //angle1+(-angle2)= Quaternion * Quaternion.inverse()
        Quaternion deltaRot = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        prevRot=ARAVRInput.RHand.rotation;

        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger))
        {
            //throwDir = new Vector3(0,1,1);
            isGrabbing = false;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObj.transform.parent = null;
            grabbedObj.GetComponent<Rigidbody>().velocity = throwDir * throwPower;

            //각속도 -> 변위 시간에 따른 회전의 변화(da/dt)
            float angle;
            Vector3 axis;
            deltaRot.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1 / Time.deltaTime) * angle * axis;
            grabbedObj.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

            grabbedObj = null;
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger))
        {

            if (isRemoteGrab)
            {
                Ray ray = new Ray();
                RaycastHit hit;
                if (Physics.SphereCast(ray,0.5f,out hit,remoteGrabDist,grabbedLayer))
                {
                    isGrabbing = true;
                    grabbedObj = hit.transform.gameObject;
                    StartCoroutine(GrabbingAnimation());
                }
                return;
            }
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
                prevPos = ARAVRInput.RHandPosition;
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    private IEnumerator GrabbingAnimation()
    {

        grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
        prevPos = ARAVRInput.RHandPosition;
        prevRot = ARAVRInput.RHand.rotation;

        Vector3 startPos = grabbedObj.transform.position;
        Vector3 endPos = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currTime = 0;
        float finishTime = 0.2f;
        float elapsedRate = currTime / finishTime;

        while (elapsedRate<1)
        {
            currTime += Time.deltaTime;

            grabbedObj.transform.position=Vector3.Lerp(startPos, endPos, elapsedRate);  

            elapsedRate = currTime / finishTime;    
            yield return null;
        }
        grabbedObj.transform.position = endPos;
        grabbedObj.transform.parent = ARAVRInput.RHand;
    }
}
