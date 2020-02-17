using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += new Vector3(0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            gameObject.transform.position -= new Vector3(0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 50f, ForceMode2D.Force);
            //ForceMode2D.Impulse
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position -= new Vector3(0, 0.1f, 0);
        }
    }
}
