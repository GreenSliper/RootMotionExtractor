using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootMotion
{
    public class RootMotionExtractorTester : MonoBehaviour
    {
        [SerializeField]
        Animator anim;
        [SerializeField]
        string animStateName;
        [SerializeField]
		AnimationClip clip;
        [SerializeField]
        RootMotionExtractor extractor;
        [SerializeField]
        bool rotLockX, rotLockY, rotLockZ;
        [SerializeField]
        bool locLockY;
        [SerializeField]
        ModelMapping mapping;
        
		TransformCurves currentCurves;
        [SerializeField]
        TransformCurves curvesCopy;

        public bool run = false;
        float playStartTime;
        Vector3 animStartPostion = Vector3.zero;
        Quaternion animStartRot;

		// Update is called once per frame
		void Update()
        {
            if (run)
            {
                if (currentCurves == null)
                {
					anim.speed = 1;
					curvesCopy = currentCurves = extractor.GetRootCurves(clip, mapping);
					anim.Play(animStateName, -1, 0);
					playStartTime = Time.time;
                    animStartPostion = anim.transform.localPosition;
					animStartRot = anim.transform.localRotation;
				}
                if (currentCurves == null || currentCurves.locationCurves.x == null || 
                    currentCurves.locationCurves.x.keys.Last().time <= Time.time - playStartTime)
                {
					currentCurves = null;
					anim.speed = 0;
                    run = false;
                    return;
                }
                float curveTime = Time.time - playStartTime;
                anim.transform.localPosition = animStartPostion + currentCurves.GetDeltaPosition(animStartRot, curveTime, lockY : locLockY);
				anim.transform.localRotation = animStartRot * currentCurves.GetDeltaRotation(curveTime, rotLockX, rotLockY, rotLockZ);
			}
        }
    }
}