using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{
    class Parameter
    {
        private static Parameter instance = null;

        public static Parameter getInstance()
        {

            if (instance == null)
            {
                instance = new Parameter();
            }
            return instance;
        }
        public void initialize(int _OPWaiting, int _mortuary, int _ward, int _silentRoom, int _triageTime, int _arriveMin, int _arriveMax, int _OPWinTime, int _OPMin, int _OPMax, int _OPDyingRate, int _OPBlockedMin, int _OPBlockedMax,int _OPBlockedRate)
        {
            OPWaiting = _OPWaiting;
            mortuary = _mortuary;
            ward = _ward;
            silentRoom = _silentRoom;

            triageTime = _triageTime;
            arriveMin = _arriveMin;
            arriveMax = _arriveMax;

            OPWinTime = _OPWinTime;
            OPMin = _OPMin;
            OPMax = _OPMax;

            OPBlockedMin = _OPBlockedMin;
            OPBlockedMax = _OPBlockedMax;

            OPDyingRate = _OPDyingRate;
            OPBlockedRate = _OPBlockedRate;
    }
        public int OPWaiting;
        public int mortuary;
        public int ward;
        public int silentRoom;

        public int triageTime;
        public int arriveMin;
        public int arriveMax;

        public int OPWinTime;
        public int OPMin;
        public int OPMax;

        public int OPBlockedMin;
        public int OPBlockedMax;

        public int OPDyingRate;
        public int OPBlockedRate;

    }
}
