using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Camera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera_Direction = 1;
        PosiVec3 = transform.position;
    }

    public float Left_End;
    public float Right_End;
    public float Speed;
    public int Camera_Direction;

    private const int Right_D = 1;
    private const int Left_D = 2;

    Vector3 PosiVec3;
    // Update is called once per frame
    void Update()
    {
        if(Camera_Direction == Right_D)
        {
            PosiVec3.x += Time.deltaTime * Speed;
            transform.position = PosiVec3;
            if(PosiVec3.x > Right_End)
               Camera_Direction = Left_D;            
        }
        if (Camera_Direction == Left_D)
        {
            PosiVec3.x -= Time.deltaTime * Speed;
            transform.position = PosiVec3;
            if (PosiVec3.x < Left_End)
                Camera_Direction = Right_D;
        }
    }
}
