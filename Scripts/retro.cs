using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���Ӿ�
public class retro : MonoBehaviour
{
    private Vector3 offset;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset = new Vector3(0.9653f, 0.811f, 0.572f);
        //ƽ�Ƹ���
        transform.position = target.transform.TransformPoint(offset);
        //��ת����
        var direction = target.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(direction);
        //����
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 61, 
            transform.localRotation.eulerAngles.z));
    }
}
