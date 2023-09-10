using System;
using System.Linq;
using UnityEngine;

namespace RootMotion
{

	public enum Axis { X, Y, Z, W }

	public class RootMotionExtractor : MonoBehaviour
	{
		public TransformCurves GetRootCurves(AnimationClip clip, ModelMapping mapping)
		{
			return new TransformCurves() 
			{ 
				locationCurves = GetClipLocationCurves(clip, mapping.locationCurveMapping, mapping.rootBoneName),
				rotationCurves = GetClipRotationCurves(clip, mapping.rotationCurveMapping, mapping.rootBoneName),
			};
		}

		public Curve3 GetClipLocationCurves(AnimationClip clip, CurveMapping3 mapping, string boneName)
		{
			#if !UNITY_EDITOR
			Debug.LogError("RootMotionExtractor is for editor use only!");
			#endif
			Curve3 result = new Curve3();
			if (!mapping.Valid())
			{
				Debug.LogError("Mapping invalid!");
				return null;
			}
			var rootCurveMoveBindings = UnityEditor.AnimationUtility.GetCurveBindings(clip).Where(x => x.path == boneName);
			foreach (var curve in rootCurveMoveBindings)
			{
				bool axisValid = true;
				Axis sourceAxis = Axis.X;
				if (curve.propertyName.EndsWith("Position.x"))
					sourceAxis = Axis.X;
				else if (curve.propertyName.EndsWith("Position.y"))
					sourceAxis = Axis.Y;
				else if (curve.propertyName.EndsWith("Position.z"))
					sourceAxis = Axis.Z;
				else
					axisValid = false;
				if (axisValid)
				{
					result.AddToCurve(sourceAxis,
						UnityEditor.AnimationUtility.GetEditorCurve(clip, curve),
						mapping, true);
					if (result.CurvesFilled)
						break;
				}
			}
			return result;

		}

		public Curve4 GetClipRotationCurves(AnimationClip clip, CurveMapping4 mapping, string boneName)
		{
			#if !UNITY_EDITOR
			Debug.LogError("RootMotionExtractor is for editor use only!");
			#endif
			Curve4 result = new Curve4();
			if (!mapping.Valid())
			{
				Debug.LogError("Mapping invalid!");
				return null;
			}
			var rootCurveRotBindings = UnityEditor.AnimationUtility.GetCurveBindings(clip).Where(x => x.path == boneName);
			foreach (var curve in rootCurveRotBindings)
			{
				bool axisValid = true;
				Axis sourceAxis = Axis.X;
				if (curve.propertyName.EndsWith("Rotation.x"))
					sourceAxis = Axis.X;
				else if (curve.propertyName.EndsWith("Rotation.y"))
					sourceAxis = Axis.Y;
				else if (curve.propertyName.EndsWith("Rotation.z"))
					sourceAxis = Axis.Z;
				else if (curve.propertyName.EndsWith("Rotation.w"))
					sourceAxis = Axis.W;
				else
					axisValid = false;
				if (axisValid)
				{
					result.AddToCurve(sourceAxis,
						UnityEditor.AnimationUtility.GetEditorCurve(clip, curve),
						mapping);
					if (result.CurvesFilled)
						break;
				}
			}
			result.SubstractStartQuaternion();
			return result;

		}
	}
}