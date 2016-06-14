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
        public void initialize(int _OPWaiting, int _mortuary, int _ward, int _church, int _triageTime, int _arriveMin, int _arriveMax, int _OPWinTime, int _OPMin, int _OPMax, int _OPDyingRate)
        {
            OPWaiting = _OPWaiting;
            mortuary = _mortuary;
            ward = _ward;
            church = _church;

            triageTime = _triageTime;
            arriveMin = _arriveMin;
            arriveMax = _arriveMax;

            OPWinTime = _OPWinTime;
            OPMin = _OPMin;
            OPMax = _OPMax;

            OPDyingRate = _OPDyingRate;
    }
        public int OPWaiting;
        public int mortuary;
        public int ward;
        public int church;

        public int triageTime;
        public int arriveMin;
        public int arriveMax;

        public int OPWinTime;
        public int OPMin;
        public int OPMax;

        public int OPDyingRate;

    }
}
