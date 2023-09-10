using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion
{
	[Serializable]
	public class Curve4
	{
		public AnimationCurve x, y, z, w;

		public bool CurvesFilled => x != null && y != null && z != null && w != null;

		public void AddToCurve(Axis sourceAxis, AnimationCurve curve, CurveMapping4 mapping)
		{
			if (mapping.xSource.sourceAxis == sourceAxis)
				x = mapping.xSource.ApplyScaleToCurve(curve, false);
			else if (mapping.ySource.sourceAxis == sourceAxis)
				y = mapping.ySource.ApplyScaleToCurve(curve, false);
			else if (mapping.zSource.sourceAxis == sourceAxis)
				z = mapping.zSource.ApplyScaleToCurve(curve, false);
			else if (mapping.wSource.sourceAxis == sourceAxis)
				w = mapping.wSource.ApplyScaleToCurve(curve, false);
		}

		Keyframe CloneChangeValue(Keyframe key, float value)
		{
			return new Keyframe(key.time, value, key.inTangent, key.outTangent, key.inWeight, key.outWeight);
		}

		public void SubstractStartQuaternion()
		{
			var startInverse = Quaternion.Inverse(new Quaternion(x.keys[0].value, y.keys[0].value, z.keys[0].value, w.keys[0].value));

			Keyframe[] xKeys = new Keyframe[x.keys.Length],
				yKeys = new Keyframe[x.keys.Length],
				zKeys = new Keyframe[x.keys.Length],
				wKeys = new Keyframe[x.keys.Length];
			for (int key = 0; key < x.keys.Length; key++)
			{
				var current = startInverse * new Quaternion(x.keys[key].value, y.keys[key].value, z.keys[key].value, w.keys[key].value);
				xKeys[key] = CloneChangeValue(x.keys[key], current.x);
				yKeys[key] = CloneChangeValue(y.keys[key], current.y);
				zKeys[key] = CloneChangeValue(z.keys[key], current.z);
				wKeys[key] = CloneChangeValue(w.keys[key], current.w);
			}
			x = new AnimationCurve(xKeys);
			y = new AnimationCurve(yKeys);
			z = new AnimationCurve(zKeys);
			w = new AnimationCurve(wKeys);
		}

		public Quaternion GetCurrent(float time, bool lockX = false, bool lockY = false, bool lockZ = false)
		{
			return new Quaternion(
				lockX ? 0 : x?.Evaluate(time) ?? 0,
				lockY ? 0 : y?.Evaluate(time) ?? 0,
				lockZ ? 0 : z?.Evaluate(time) ?? 0,
				w?.Evaluate(time) ?? 0);
		}
	}
}