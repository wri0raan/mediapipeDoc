using UnityEngine;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;

public class PosePointDrawer_Preset1 : MonoBehaviour
{
    [Header("필수 참조")]
    public PoseLandmarkerRunner runner;

    public RectTransform pointA;
    public RectTransform pointB;

    [Header("포인트에 사용할 랜드마크 인덱스")]
    public int indexA = 4;
    public int indexB = 6;

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

        NormalizedLandmark la = pose.landmarks[indexA];
        NormalizedLandmark lb = pose.landmarks[indexB];

        lock (_lock)
        {
            _ax = la.x;
            _ay = la.y;
            _bx = lb.x;
            _by = lb.y;
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
