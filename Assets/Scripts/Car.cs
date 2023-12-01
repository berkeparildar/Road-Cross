using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject model;
    // Start is called before the first frame update
    private void Awake()
    {
        model = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    public void SetDirection(int spawnPosition)
    {
        if (spawnPosition == 30)
        {
            model.transform.rotation = Quaternion.Euler(0, -90, 0);
            direction = Vector3.left;
        }
        else
        {
            model.transform.rotation = Quaternion.Euler(0, 90, 0);
            direction = Vector3.right;
        }
    }

    public void SetSpeed(float carSpeed)
    {
        speed = carSpeed;
    }
}
