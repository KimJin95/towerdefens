using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ź�� �浹�� ������ �� �ֺ��� �ִ� ��е��� �����ϰ� �ʹ�.
// �ʿ� �Ӽ�: ���� ȿ��, ���� ����
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
