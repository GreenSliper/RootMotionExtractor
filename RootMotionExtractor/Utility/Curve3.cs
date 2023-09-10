using RootMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion
{
	[Serializable]
	public class Curve3
	{
		public AnimationCurve x, y, z;
		public bool CurvesFilled => x != null && y != null && z != null;

		public void AddToCurve(Axis sourceAxis, AnimationCurve curve, CurveMapping3 mapping, bool zeroStartPosition)
		{
			if (mapping.xSource.sourceAxis == sourceAxis)
				x = mapping.xSource.ApplyScaleToCurve(curve, zeroStartPosition);
			else if (mapping.ySource.sourceAxis == sourceAxis)
				y = mapping.ySource.ApplyScaleToCurve(curve, zeroStartPosition);
			else if (mapping.zSource.sourceAxis == sourceAxis)
				z = mapping.zSource.ApplyScaleToCurve(curve, zeroStartPosition);
		}

		public Vector3 GetCurrent(float time, bool lockX = false, bool lockY = false, bool lockZ = false)
		{
			return new Vector3(
				lockX ? 0 : x?.Evaluate(time) ?? 0,
				lockY ? 0 : y?.Evaluate(time) ?? 0,
				lockZ ? 0 : z?.Evaluate(time) ?? 0);
		}
	}
}