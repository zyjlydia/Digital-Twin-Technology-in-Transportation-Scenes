using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��ʻԱ�ӽ�
public class DriveCamera : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private GameObject target;
    [SerializeField] private float translateSpeed = 100f;
    [SerializeField] private float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(-0.02f, 1f, -0.17f);
    }

    private void FixedUpdate()
    {
        //ƽ�Ƹ���
        var targetPosition = target.transform.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        //��ת����
        var direction = target.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(direction);
        //����
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 6.7f,
            transform.localRotation.eulerAngles.z));
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
