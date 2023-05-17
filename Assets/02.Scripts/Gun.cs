using UnityEngine;
using System.Collections;
// 사용자가 발사 버튼을 누르면 총을 쏘고 싶다.
// 필요 속성: 총알 파편, 총알 파편 효과, 총알 발사 사운드
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
