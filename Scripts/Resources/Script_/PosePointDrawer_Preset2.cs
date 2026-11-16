using UnityEngine;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;

public class PosePointDrawer_Preset2 : MonoBehaviour
{
    [Header("필수 참조")]
    public PoseLandmarkerRunner runner;

    public RectTransform pointA;
    public RectTransform pointB;

    [Header("포인트에 사용할 랜드마크 인덱스")]
    public int indexA = 4;
    public int indexB = 6;

    [Header("손목→팔꿈치 방향 내관 비율 (0~1)")]
    [Range(0f, 1f)] public float tA = 0.25f;
    [Range(0f, 1f)] public float tB = 0.25f;

    [Header("팔-몸 위치 방향 오프셋 비율 (0이면 중앙)")]
    public float sideOffsetRatio = 0.00f;

    [Header("각 포인트 미세 오프셋 (px 단위)")]
    public Vector2 offsetA = Vector2.zero;
    public Vector2 offsetB = Vector2.zero;

    [Header("포즈가 그려질 스크린 (Annotatable Screen)")]
    public RectTransform screenRect;

    private readonly object _lock = new object();
    private bool _hasNewPose = false;
    private float _ax, _ay;
    private float _bx, _by;

    private void OnEnable()
    {
        if (runner != null)
        {
            runner.Result += OnPoseUpdated;
        }
    }

    private void OnDisable()
    {
        if (runner != null)
        {
            runner.Result -= OnPoseUpdated;
        }
    }

    private void OnPoseUpdated(PoseLandmarkerResult result, long ts)
    {
        if (result.poseLandmarks == null || result.poseLandmarks.Count == 0)
            return;

        var pose = result.poseLandmarks[0];
        if (pose.landmarks == null || pose.landmarks.Count == 0)
            return;

        if (indexA < 0 || indexA >= pose.landmarks.Count) return;
        if (indexB < 0 || indexB >= pose.landmarks.Count) return;

        var wristA = pose.landmarks[indexA];
        var wristB = pose.landmarks[indexB];
        var elbowA = pose.landmarks[13];
        var elbowB = pose.landmarks[14];

        float baseAx = wristA.x + tA * (elbowA.x - wristA.x);
        float baseAy = wristA.y + tA * (elbowA.y - wristA.y);

        float baseBx = wristB.x + tB * (elbowB.x - wristB.x);
        float baseBy = wristB.y + tB * (elbowB.y - wristB.y);

        float nxA = baseAx;
        float nyA = baseAy;
        float nxB = baseBx;
        float nyB = baseBy;

        if (sideOffsetRatio != 0f)
        {

            float vxA = elbowA.x - wristA.x;
            float vyA = elbowA.y - wristA.y;
            float lenA = Mathf.Sqrt(vxA * vxA + vyA * vyA);
            if (lenA > 1e-5f)
            {
                float dxA = vxA / lenA;
                float dyA = vyA / lenA;

                float pxA = -dyA;
                float pyA = dxA;

                nxA += sideOffsetRatio * pxA;
                nyA += sideOffsetRatio * pyA;
            }

            float vxB = elbowB.x - wristB.x;
            float vyB = elbowB.y - wristB.y;
            float lenB = Mathf.Sqrt(vxB * vxB + vyB * vyB);
            if (lenB > 1e-5f)
            {
                float dxB = vxB / lenB;
                float dyB = vyB / lenB;
                float pxB = -dyB;
                float pyB = dxB;

                nxB -= sideOffsetRatio * pxB;
                nyB -= sideOffsetRatio * pyB;
            }
        }

        lock (_lock)
        {
            _ax = nxA;
            _ay = nyA;
            _bx = nxB;
            _by = nyB;
            _hasNewPose = true;
        }
    }

    private void Update()
    {
        bool has;
        float ax = 0, ay = 0, bx = 0, by = 0;

        lock (_lock)
        {
            has = _hasNewPose;
            if (has)
            {
                _hasNewPose = false;
                ax = _ax;
                ay = _ay;
                bx = _bx;
                by = _by;
            }
        }

        if (!has) return;

        MovePoint(pointA, ax, ay, offsetA);
        MovePoint(pointB, bx, by, offsetB);
    }

    private void MovePoint(RectTransform rt, float nx, float ny, Vector2 offset)
    {
        if (rt == null || screenRect == null)
            return;

        var rect = screenRect.rect;

        float x = (nx - 0.5f) * rect.width;
        float y = (0.5f - ny) * rect.height;

        rt.anchoredPosition = new Vector2(x, y) + offset;
    }
}
