using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public GameObject[] _camera;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        _camera[0].SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //����V��-�л��ӽ�
        if (Input.GetKeyDown(KeyCode.V))
        {
            _camera[i % _camera.Length].SetActive(false);
            i++;
            _camera[i % _camera.Length].SetActive(true);
        }
    }
}
