using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject target;
    public GameObject bullet;
    public float speed;
    public float SpeedRotation;

    void Update()
    {
        if (target)
        {
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, target.transform.position,
                    Time.deltaTime * speed);

            LookAtEnemy(target, bullet, rb);
        }
        else
        {
            Destroy(bullet);
        }
    }

    private void LookAtEnemy(GameObject target, GameObject bull, Rigidbody2D arr)
    {
        /*arrow - transform �������, ������� ������ �������(����� ������������ ������ transform), 
        target - transform, ����������, ����.
        arr - Rigidbody2d �������, �� ������� ����� ������.
        SpeedRotation - �������� ��������
        ������ MoveRotation ����� ������������ ����� ������ ������.*/
        var turn = Quaternion.Lerp(bull.transform.rotation, Quaternion.LookRotation(Vector3.forward,
            target.transform.position - bull.transform.position), Time.deltaTime * SpeedRotation);
        arr.MoveRotation(turn.eulerAngles.z);
    }
}
