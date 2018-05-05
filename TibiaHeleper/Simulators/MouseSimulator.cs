using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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


        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out PointInter lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct PointInter
        {
            public int X;
            public int Y;
            public static explicit operator Point(PointInter point) => new Point(point.X, point.Y);
        }


        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x202;
        const uint WM_RBUTTONDOWN = 0x0204;
        const uint WM_RBUTTONUP = 0x0205;
        const uint WM_MOUSEMOVE = 0x0200;
        const uint WM_SETCURSOR = 0x0020;



        public static void test()
        {
            SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, (IntPtr)0, MakeLParam(87,782));
            SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, (IntPtr)0, MakeLParam(87,782));
        }

        public static void click(int XPos, int YPos, bool isRightClick = false)
        {
            SimulatorSynchronisation.semaphore.WaitOne();
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
            SimulatorSynchronisation.semaphore.Release();
        }

        public static void drag(int fromXPos, int fromYPos,int toXPos,int toYPos , bool moveRealMouse=false)
        {
            SimulatorSynchronisation.semaphore.WaitOne();
          

            SendMessage(proc.MainWindowHandle, WM_LBUTTONDOWN, (IntPtr)0, MakeLParam(fromXPos, fromYPos));

            PointInter cursor;
            GetCursorPos(out cursor);

            if (moveRealMouse)
            {
                //It is necessary because Tibia has to change cursor and this happens only when real mouse cursor is moved
                SetCursorPos(toXPos, toYPos);
                Thread.Sleep(50);
            }
           

            SendMessage(proc.MainWindowHandle, WM_LBUTTONUP, (IntPtr)0, MakeLParam(toXPos, toYPos));

            if(moveRealMouse)
                SetCursorPos(cursor.X, cursor.Y);

            SimulatorSynchronisation.semaphore.Release();

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
