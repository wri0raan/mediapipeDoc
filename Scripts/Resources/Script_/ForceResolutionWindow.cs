using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class ForceResolutionWindow : MonoBehaviour
{
    [HideInInspector] public int targetWidth = 1080;
    [HideInInspector] public int targetHeight = 1920;
    [HideInInspector] public bool centerWindow = true;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll")] private static extern bool MoveWindow(
        IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint
    );

    private struct RECT { public int Left, Top, Right, Bottom; }

    IntPtr hWnd;

    void Start()
    {
        hWnd = GetActiveWindow();
        ForceSize(true);
    }

    void Update()
    {
        ForceSize(false);
    }

    void ForceSize(bool first)
    {
        if (hWnd == IntPtr.Zero) return;

        RECT rect;
        if (!GetWindowRect(hWnd, out rect)) return;

        int curW = rect.Right - rect.Left;
        int curH = rect.Bottom - rect.Top;

        if (curW != targetWidth || curH != targetHeight || first)
        {
            int x = rect.Left;
            int y = rect.Top;

            if (centerWindow)
            {
                int monitorW = Display.main.systemWidth;
                int monitorH = Display.main.systemHeight;
                x = (monitorW - targetWidth) / 2;
                y = (monitorH - targetHeight) / 2;
            }

            MoveWindow(hWnd, x, y, targetWidth, targetHeight, true);
        }
    }
#endif
}
