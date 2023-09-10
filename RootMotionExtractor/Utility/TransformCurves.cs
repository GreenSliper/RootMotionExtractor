using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion
{
    [Serializable]
    public class TransformCurves
    {
        public Curve3 locationCurves;
		public Curve4 rotationCurves;

        public Vector3 GetDeltaPosition(Quaternion startRotation, float time, bool lockX = false, bool lockY = false, bool lockZ = false)
        {
            return startRotation * locationCurves.GetCurrent(time, lockX, lockY, lockZ);
        }

        public Quaternion GetDeltaRotation(float time, bool lockX = false, bool lockY = false, bool lockZ = false)
            => rotationCurves.GetCurrent(time, lockX, lockY, lockZ);
    }
}