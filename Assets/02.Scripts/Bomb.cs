using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 폭탄이 충돌해 폭발할 때 주변에 있는 드론들을 제거하고 싶다.
// 필요 속성: 폭발 효과, 폭발 영역
public class Bomb : MonoBehaviour
{
    Transform explosion;

    float range = 5;
    private void Start()
    {
        explosion = GameObject.Find("Explosion").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Drone");
        Collider[] colls = Physics.OverlapSphere(transform.position, range, layerMask);

       foreach (var coll in colls)
        {
            Destroy(coll.gameObject);
        }

        explosion.position = transform.position;
        explosion.GetComponent<ParticleSystem>().Play();
        explosion.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
