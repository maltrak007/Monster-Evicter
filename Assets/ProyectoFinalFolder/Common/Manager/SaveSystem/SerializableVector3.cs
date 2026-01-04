using System;
using UnityEngine;

namespace ProyectoFinalFolder.Common.Manager.SaveSystem
{
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public static implicit operator Vector3(SerializableVector3 sVec) => sVec.ToVector3();
        public static implicit operator SerializableVector3(Vector3 vec) => new SerializableVector3(vec);
    }
}