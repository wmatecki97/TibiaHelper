using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    public static class Constants
    {

        public static int GameWindowWidthSquares { get; set; }
        public static int GameWindowHeightSquares { get; set; }
        public static int ShieldYOffset { get; }
        public static int ShieldXOffset { get;}
        public static int BackpackYOffset { get; }
        public static int MaximizeEQButtonXOffset { get; }
        public static int MaximizeEQButtonYOffset { get; }
        public static int AttackModeXOffset { get; }
        public static int[] AttackMode_YOffset { get; }
        public static int FollowTargetModeXOffset { get; }
        public static int[] FollowTargetModeYOffset { get; }

        public static int FirstOpenedWindowYOffset { get; }
        public static int ItemInOpenedWindowYOffset { get; }
        public static int ItemInOpenedWindowFromRightOffset { get; }
        public static int ItemInOpenWindowWidth { get; }
        public static int OpenedWindowScrollUpButtonYOffset { get; }
        public static int OpenedWindowScrollDownButtonFromBottomYOffset { get; }
        public static int OpenedWindowScrollFromRightXOffset { get; }
        public static int FullLineScrollClickCount { get; }
        public static int OpenedWindowResizeFromBottomYOffset { get; }
        public static int OpenedWindowMinimumHeight { get; }
        public static int OpenedWindowCloseButtonFromLeftXOffset { get; }
        public static int OpenedWindowCloseButtonYOffset { get; }
        public static int OpenedWindowNavigateButtonsYOffset { get; }
        public static int OpenedWindowPreviousButtonFromRightXOffset { get; }


        public const int TradeWindowSellOrOKButtonXFromRightOffset = -30;
        public const int TradeWindowSellButtonYOffset = 25;
        public const int TradeWindowFirstItemYOffset = 50;
        public const int TradeWindowSecondItemYOffset = 65;
        public const int ThirdItemFromListYOffset = 80;
        public const int TradeWindowScrollDownButtonYOffset = 82;
        public const int TradeWindowItemCountSliderYPosition = 100;
        public const int TradeWindowItemCountSliderLeftXPosition = -153;
        public const int TradeWindowItemCountMoreButtonXPosition = -60;
        public const int TradeWindowOKButtonYOffset = 140;





        static Constants()
        {

            GameWindowHeightSquares = 11;
            GameWindowWidthSquares = 15;
            ShieldYOffset = 75;
            ShieldXOffset = -73;
            BackpackYOffset = 38;
            MaximizeEQButtonXOffset = -160;
            MaximizeEQButtonYOffset = 10;

            AttackModeXOffset = -40;
            AttackMode_YOffset = new int[3];
            AttackMode_YOffset[0] = -5;
            AttackMode_YOffset[1] = 15;
            AttackMode_YOffset[2] = 35;

            FollowTargetModeXOffset = -20;
            FollowTargetModeYOffset = new int[2];
            FollowTargetModeYOffset[0] = 10;
            FollowTargetModeYOffset[1] = 30;

            FirstOpenedWindowYOffset = 360;
            ItemInOpenedWindowYOffset = 35;
            OpenedWindowScrollUpButtonYOffset = 20;
            OpenedWindowResizeFromBottomYOffset = -1;
            OpenedWindowScrollDownButtonFromBottomYOffset = -12;
            OpenedWindowScrollFromRightXOffset = -10;
            FullLineScrollClickCount = 4;
            OpenedWindowMinimumHeight = 60;
            ItemInOpenedWindowFromRightOffset = -40;
            ItemInOpenWindowWidth = 37;
            OpenedWindowCloseButtonFromLeftXOffset = -10;
            OpenedWindowCloseButtonYOffset = 7;
            OpenedWindowNavigateButtonsYOffset = 2;
            OpenedWindowPreviousButtonFromRightXOffset = -36;

        }


    }
}
