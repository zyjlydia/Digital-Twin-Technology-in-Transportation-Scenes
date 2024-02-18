using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retroL : MonoBehaviour
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
        //Æ½ÒÆ¸úËæ
        //offset = new Vector3(0.87f, 0.3f, 0.54f);
        offset = new Vector3(-0.914f, 0.771f, 0.572f);
        transform.position = target.transform.TransformPoint(offset);
        //Ðý×ª¸úËæ
        var direction = target.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(direction);
        //ÐÞÕý
        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 298.42f,
            transform.localRotation.eulerAngles.z));
    }
}
