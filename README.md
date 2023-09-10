# RootMotionExtractor
Tool for extracting &amp; playing custom root motion curves. Exports single bone translation &amp; rotation, easy to apply at runtime. 

Use <b>RootMotionExtractor.GetRootCurves</b> to generate root curves for your animation. You can store (serializable) <b>TransformCurves</b> for later runtime usage.  
- <b>locationCurves</b> contain delta position data of <i>root bone</i> relative to its first frame local position
- <b>rotationCurves</b> contain quaternion data describing <i>root bone</i> rotation relative to start local rotation

It's preferable to use TransformCurves <b>GetDeltaPosition & GetDeltaRotation</b> methods to get rotation & location at the given time during animation.   
<b>RootMotionExtractorTester</b> has an example of applying translation & rotation. Release version contains test model & animation.
