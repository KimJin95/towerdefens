using UnityEngine;
using System.Collections;
// ����ڰ� �߻� ��ư�� ������ ���� ��� �ʹ�.
// �ʿ� �Ӽ�: �Ѿ� ����, �Ѿ� ���� ȿ��, �Ѿ� �߻� ����
public class Gun : MonoBehaviour
{
    [SerializeField] Transform crossHair;
    [SerializeField] ParticleSystem bulletEffect;
    [SerializeField] AudioSource bulletAudio;


    private void Update()
    {
        ARAVRInput.DrawCrosshair(crossHair);

        shot();
    }

    void shot()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
           

            bulletAudio.Stop();
            bulletAudio.Play();

            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hit;

            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;

            if (Physics.Raycast(ray, out hit, 200,~layerMask))
            {

                bulletEffect.Stop();
                bulletEffect.Play();

                bulletEffect.transform.position = hit.point;
                bulletEffect.transform.forward = hit.normal;
            }
        }
    }
}
