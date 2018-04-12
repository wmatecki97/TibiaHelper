using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Simulators
{
    static class MouseSimulator
    {
        static Process proc;
        static MouseSimulator()
        {
            proc = GetData.getProcess();
        }

        private static int leftTopFieldXPosition { get { return GetData.MyXPosition - 7; } }
        private static int leftTopFieldYPosition { get { return GetData.MyYPosition - 5; } }
        private static int fieldPixelSize { get { return GetData.GameWindowHeight / Constants.GameWindowHeightSquares; } }


        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x202;
        const uint WM_RBUTTONDOWN = 0x0204;
        const uint WM_RBUTTONUP = 0x0205;


        public static void test()
        {
            SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, (IntPtr)0, MakeLParam(87,782));
            SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, (IntPtr)0, MakeLParam(87,782));
        }

        public static void click(int XPos, int YPos, bool isRightClick = false)
        {
            if (isRightClick)
            {
                SendMessage(proc.MainWindowHandle, WM_RBUTTONDOWN, (IntPtr)0, MakeLParam(XPos, YPos));
                SendMessage(proc.MainWindowHandle, WM_RBUTTONUP, (IntPtr)0, MakeLParam(XPos, YPos));
            }
            else
            {
                SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, (IntPtr)0, MakeLParam(XPos, YPos));
                SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, (IntPtr)0, MakeLParam(XPos, YPos));
            }
        }       

        public static void clickOnField(int xPosition, int yPosition, bool isRightClick = false)
        {
            int xToMove = xPosition - leftTopFieldXPosition;
            int yToMove = yPosition - leftTopFieldYPosition;

            //setting start position to a centre of first field in left up square
            int XPixelPosition = GetData.GameWindowDistanceFromLeft + fieldPixelSize / 2;
            int YPixelPosition = fieldPixelSize / 2;

            //geting coordinates form centre of destination field
            XPixelPosition += xToMove * fieldPixelSize;
            YPixelPosition += yToMove * fieldPixelSize;

            click(XPixelPosition, YPixelPosition, isRightClick);

        }


        public static IntPtr MakeLParam(int Xpos, int Ypos)
        {
            return (IntPtr)((Ypos << 16) | Xpos);
        }
    }
}
