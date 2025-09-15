using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BorderlessWindow : MonoBehaviour
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const uint WS_BORDER = 0x00800000;
    private const uint WS_POPUP = 0x80000000;
    private const uint WS_VISIBLE = 0x10000000;
    private const uint WS_MINIMIZEBOX = 0x00020000;
    private const uint WS_MAXIMIZEBOX = 0x00010000;
    private const uint WS_SIZEBOX = 0x00040000;
    private const uint WS_OVERLAPPEDWINDOW = WS_BORDER | WS_POPUP | WS_VISIBLE | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SIZEBOX;

    private const int GWL_STYLE = -16;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_SHOWWINDOW = 0x0040;

    private const int DWMWA_BORDER_COLOR = 34; // DWM window attribute for border color

    private IntPtr unityWindowHandle;

    void Start()
    {
        if (Application.isEditor)
        {
            return; // Exit if running in the editor to prevent changes to the Unity editor window
        }

        unityWindowHandle = GetUnityWindowHandle();
        if (unityWindowHandle == IntPtr.Zero)
        {
            Debug.LogError("Unity window handle not found.");
            return;
        }

        ApplyWindowStyle();
    }

    private IntPtr GetUnityWindowHandle()
    {
        // Use the window title to find the Unity window handle
        string windowTitle = Application.productName; // This should match the window title of your Unity game
        IntPtr hWnd = FindWindow(null, windowTitle);

        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("Unity window handle not found.");
        }

        return hWnd;
    }

    private void ApplyWindowStyle()
    {
        uint style = GetWindowLong(unityWindowHandle, GWL_STYLE);
        style &= ~WS_OVERLAPPEDWINDOW; // Remove the overlapped window style
        style |= WS_POPUP; // Add popup style to create a borderless window

        SetWindowLong(unityWindowHandle, GWL_STYLE, style);
        SetWindowPos(unityWindowHandle, IntPtr.Zero, 0, Screen.currentResolution.height - 250 - 100, Screen.currentResolution.width, 300, SWP_NOZORDER | SWP_SHOWWINDOW);

        // Set border color to black
        int borderColor = 0x000000; // Black color in RGB
        DwmSetWindowAttribute(unityWindowHandle, DWMWA_BORDER_COLOR, ref borderColor, sizeof(int));
    }
}
