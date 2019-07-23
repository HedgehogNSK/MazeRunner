using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hedge
{ namespace Tools
    {
        static public class VectorExtension 
        {
            static public Vector3 X(this Vector3 v)
            {
                return new Vector3(v.x, 0, 0);
            }
            static public Vector3 Y(this Vector3 v)
            {
                return new Vector3(0, v.y, 0);
            }
            static public Vector3 Z(this Vector3 v)
            {
                return new Vector3(0, 0, v.z);
            }

            static public Vector3 XY(this Vector3 v)
            {
                return new Vector3(v.x, v.y, 0);
            }
            static public Vector3 YZ(this Vector3 v)
            {
                return new Vector3(0, v.y, v.z);
            }
            static public Vector3 XZ(this Vector3 v)
            {
                return new Vector3(v.x, 0, v.z);
            }



        }
    }
}

