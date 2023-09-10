using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion
{
	[Serializable]
	public class CurveMapping3
	{
		[Serializable]
		public class AxisMapping
		{
			public Axis sourceAxis;
			/// <summary>
			/// x is time, y is value
			/// </summary>
			public Vector2 scale;

			public AnimationCurve ApplyScaleToCurve(AnimationCurve curve, bool zeroStartPosition)
			{
				Keyframe[] keys = new Keyframe[curve.keys.Length];
				float offset = 0;
				if (zeroStartPosition)
					offset = -curve.keys[0].value;
				for (int keyIndex = 0; keyIndex < curve.keys.Length; keyIndex++)
					keys[keyIndex] = new Keyframe(
						curve.keys[keyIndex].time * scale.x,
						(curve.keys[keyIndex].value + offset) * scale.y,
						curve.keys[keyIndex].inTangent * scale.y,
						curve.keys[keyIndex].outTangent * scale.y,
						curve.keys[keyIndex].inWeight,
						curve.keys[keyIndex].outWeight);
				return new AnimationCurve(keys);
			}
		}

		public AxisMapping xSource, ySource, zSource;
		public virtual bool Valid()
		{
			//target axises must differ
			return !(xSource.sourceAxis == ySource.sourceAxis ||
			   xSource.sourceAxis == zSource.sourceAxis ||
			   ySource.sourceAxis == zSource.sourceAxis);
		}
	}

	[Serializable]
	public class CurveMapping4 : CurveMapping3
	{
		public AxisMapping wSource;
		public override bool Valid()
		{
			return base.Valid()
				&& xSource.sourceAxis != wSource.sourceAxis && ySource.sourceAxis != wSource.sourceAxis && zSource.sourceAxis != wSource.sourceAxis;
		}
	}

	[Serializable]
	public class ModelMapping
	{
		public CurveMapping3 locationCurveMapping;
		public CurveMapping4 rotationCurveMapping;
		public string rootBoneName;
		public bool Valid() => locationCurveMapping.Valid() && rotationCurveMapping.Valid();
	}
}