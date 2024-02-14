using HP.Omnicept.Messaging.Messages;
using VRCFaceTracking;
using VRCFaceTracking.Core.Types;
using Eye = HP.Omnicept.Messaging.Messages.Eye;

namespace VRCFTOmniceptModule;

public class VRCFTEyeTracking
{
    public class VRCFTEye
    {        
        public Vector2 Look = new();
        
        public float PupilDilate;

        private static float ProperRangeDilate(float dilation) => (dilation - 1.5f) / 6.5f;

        public void Update(Eye data)
        {
            if (data.Gaze.Confidence >= 0.25f)
            {
                Look.x = data.Gaze.X * -1;
                Look.y = data.Gaze.Y;
            }
            if(data.PupilDilationConfidence >= 0.25f)
                PupilDilate = ProperRangeDilate(data.PupilDilation);
        }


        public enum EyeType
        {
            Left,
            Right,
            Combined
        }
    }

    public class VRCFTEyeTrackingData
    {
        public VRCFTEye LeftEye = new();
        public VRCFTEye RightEye = new();

        public void Update(EyeTracking data)
        {
            LeftEye.Update(data.LeftEye);
            RightEye.Update(data.RightEye);
        }
    }
    
    public static void UpdateEyeTrackingData(VRCFTEyeTrackingData data)
    {
        // LeftEye
        UnifiedTracking.Data.Eye.Left.Gaze = data.LeftEye.Look;
        UnifiedTracking.Data.Eye.Left.PupilDiameter_MM = data.LeftEye.PupilDilate;
        // RightEye
        UnifiedTracking.Data.Eye.Right.Gaze = data.RightEye.Look;
        UnifiedTracking.Data.Eye.Right.PupilDiameter_MM = data.RightEye.PupilDilate;
    }
}