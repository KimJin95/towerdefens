using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform damageUI;
    [SerializeField] Image damageImage;

    [SerializeField] int initHp = 10;
    int hp = 0;
    //프로퍼티

    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;

            StopCoroutine("DamageEvent");
            StartCoroutine("DamageEvent");
            if (hp <= 0)
            {
                print("Gameover");
            }
        }
    }

    float damageTime = 0.1f;

    private void Start()
    {
        hp = initHp;
        float z = Camera.main.nearClipPlane + 0.01f;
        damageUI.parent = Camera.main.transform;
        damageUI.localPosition = new Vector3(0, 0, z);


        damageImage.enabled = false;
    }
    IEnumerator DamageEvent()
    {
        damageImage.enabled = true;
        yield return new WaitForSeconds(damageTime);
        damageImage.enabled = false;
    }
}
