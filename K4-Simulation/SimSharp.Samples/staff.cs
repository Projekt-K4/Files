/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{
    class Staff
    {

        //enum - environment?
        private enum StaffState
        {
            working,
            arriving,
            available,
            notAvailable //e.g. on Holidays but not necessary?
        };

        String state_s = StaffState.available.ToString();        //or something different

        //String state_s=get_state();

        //static String get_state()
        //{
        //    Random random = new Random();
        //    int random_number = random.Next(1, 4);
        //    if (random_number == 1)
        //    {
        //        return state.arriving.ToString();
        //    }
        //    else if (random_number == 2)
        //    {
        //        return state.working.ToString();
        //    }
        //    else if (random_number == 3)
        //    {
        //        return state.available.ToString();
        //    }
        //    else //if (random_number == 4)
        //    {
        //        return state.notAvailable.ToString();
        //    }


        //}

    }

    class Doctor : Staff
    {
        private bool is_TriageDoctor;

        enum Type
        {
            NeuroSurgeon,       //Neurochirurg
            Anaesthesia,        //Anästhesist
            TraineeDoctor,      //Turnusarzt
            EmergencySurgeon    //Unfallchirurg
        };

        String Type_s;

        //enum Spezialiced_at

    }

    class AuxiliaryStaff : Staff        //Hilfspersonal
    {

        private enum AuxType
        {
            NurseAnesthetist,       //Anästhesiepfleger
            IntensiveCareNurse,     //Intensivpfleger
            SurgeryAssistant,       //OP-Gehilfe
            TheatreNurse,           //OP-Pfleger
            Nurse,                  //Pfleger
            BringAndGetService,     //Hol- und Bringsdienst
            //Beidienst,              //Beidienst? - englisch?
            Cleaner                 //Putzkraft
        }

        String AuxType_s;

        private bool is_TriageAssistant = false;
    }

}*/
