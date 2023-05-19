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

    //커브의 부드러운 정도
    int lineSmooth = 40;
    //커브 길이
    float curveLength = 50;
    //커브 중력
    float curvegarvity = -60;
    //곡선 시뮬레이션의 간격 및 시간
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
        Vector3 dir = ARAVRInput.LHandDirection * curveLength; //선이 진행할 방향
        Vector3 pos = ARAVRInput.LHandPosition; //선의 첫번째 위치
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
