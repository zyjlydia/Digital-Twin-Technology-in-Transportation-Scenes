using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �����˳��ӽ�
public class FollowCamera : MonoBehaviour
{
    private Vector3 offset;//ƫ����
    [SerializeField] private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        offset = new Vector3(0, 4.14f, -7.26f);
        transform.position = target.transform.position + offset;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
