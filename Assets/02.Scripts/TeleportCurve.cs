using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    [SerializeField] Transform teleportCircle;

    Vector3 originScale = Vector3.one * 0.01f;

    LineRenderer myline;

    //Ŀ���� �ε巯�� ����
    int lineSmooth = 40;
    //Ŀ�� ����
    float curveLength = 50;
    //Ŀ�� �߷�
    float curvegarvity = -60;
    //� �ùķ��̼��� ���� �� �ð�
    float simulateTime = 0.02f;

    List<Vector3> lines = new List<Vector3>();
    private void Start()
    {
        teleportCircle.gameObject.SetActive(false);
        myline = GetComponent<LineRenderer>();
        myline.enabled = false;
        myline.startWidth = 0.001f;
        myline.endWidth = 0.2f;
    }

    private void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            myline.enabled = true;
        }

        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            myline.enabled = false;

            if (teleportCircle.gameObject.activeSelf)
            {
                GetComponent<CharacterController>().enabled = false;

                transform.position = teleportCircle.position + Vector3.up;

                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircle.gameObject.SetActive(false);
        }

        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }
    }

    private void MakeLines()
    {
        lines.Clear();
        Vector3 dir = ARAVRInput.LHandDirection * curveLength; //���� ������ ����
        Vector3 pos = ARAVRInput.LHandPosition; //���� ù��° ��ġ
        lines.Add(pos);

        for (int i = 0; i < lineSmooth; i++)
        {
            Vector3 lastpos = pos;
            dir.y += curvegarvity * simulateTime;
            pos += dir * simulateTime;
            if (CheckHitRay(lastpos, ref pos))
            {
                lines.Add(pos); break;
            }
            else
            {
                teleportCircle.gameObject.SetActive(false);
            }
            lines.Add(pos);
        }
        myline.positionCount = lines.Count;
        myline.SetPositions(lines.ToArray());
    }

    bool CheckHitRay(Vector3 lastpos, ref Vector3 pos)
    {
        Vector3 rayDir = pos - lastpos;
        Ray ray = new Ray(lastpos, rayDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDir.magnitude))
        {
            pos = hit.point;
            int layer = LayerMask.NameToLayer("Terrain");
            if (hit.transform.gameObject.layer == layer)
            {
                teleportCircle.gameObject.SetActive(true);
                teleportCircle.position = hit.point;
                teleportCircle.forward = hit.normal;
                float dist = (pos - ARAVRInput.LHandPosition).magnitude;
                teleportCircle.localScale = originScale*Mathf.Max(1,dist);

            }
            return true;
        }
        return false;
    }
}
