using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    [SerializeField] Transform teleportCircle;

    LineRenderer myline;
    Vector3 originScale = Vector3.one * 0.01f;

    private void Start()
    {
        teleportCircle.gameObject.SetActive(false);
        myline = GetComponent<LineRenderer>();
        myline.enabled = false;
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
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hit;

            int layerMask = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out hit, 200, layerMask))
            {
                myline.SetPosition(0, ray.origin);
                myline.SetPosition(0, hit.point);

                teleportCircle.gameObject.SetActive(true);
                teleportCircle.position = hit.point;
                teleportCircle.forward = hit.normal;
                teleportCircle.localScale = originScale * Mathf.Max(1, hit.distance);
            }
            else
            {
                myline.SetPosition(0, ray.origin);
                myline.SetPosition(1, ray.origin + ARAVRInput.LHandPosition * 200);

                teleportCircle.gameObject.SetActive(false);
            }
        }
    }
}
