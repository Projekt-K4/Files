using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSharp.Samples
{
    public class Staff
    {
        public Staff(String value)
        {
            ID = value;
        }
        public Staff(int value)
        {
            ID = value.ToString();
        }
        public String ID { get; set; }
        public override String ToString()
        {
            return ID.ToString();
        }
    }
    public class OP
    {
        public OP(String value)
        {
            ID = value;
        }
        public String ID { get; set; }
        public override String ToString()
        {
            return ID.ToString();
        }
    }
    class RSStore
    {
        private static RSStore instance = null;

        public static RSStore getInstance()
        {

            if (instance == null)
            {
                instance = new RSStore();
            }
            return instance;
        }
        public Store OPStore;
        public Store ChirurgStore;
        public Store NurseStore;
        public Store SupportStore;
        public Store AnesthesistStore;
        public Store AnesthesistNurseStore;
        public Store RTAStore;

        public void initStores(Environment env, int chirurgs, int nurses, int support, int anesthesists, int anesthesistsNurse, int rtas, int ops)
        {
            int i = 1;
            OPStore = new Store(env, ops);
            ChirurgStore = new Store(env, chirurgs);
            NurseStore = new Store(env, nurses);
            SupportStore = new Store(env, support);
            AnesthesistStore = new Store(env, anesthesists);
            AnesthesistNurseStore = new Store(env, anesthesistsNurse);
            RTAStore = new Store(env, rtas);
            while (i <= chirurgs || i <= nurses || i <= support || i <= anesthesists || i <= anesthesistsNurse || i <= rtas || i <= ops)
            {
                if (i <= chirurgs)
                {
                    ChirurgStore.Put(new Staff(i));
                }
                if (i <= nurses)
                {
                    NurseStore.Put(new Staff(i));
                }
                if (i <= support)
                {
                    SupportStore.Put(new Staff(i));
                }
                if (i <= anesthesists)
                {
                    AnesthesistStore.Put(new Staff(i));
                }
                if (i <= anesthesistsNurse)
                {
                    AnesthesistNurseStore.Put(new Staff(i));
                }
                if (i <= rtas)
                {
                    RTAStore.Put(new Staff(i));
                }
                if (i <= ops)
                {
                    OPStore.Put(new Staff(i));
                }
                ++i;
            }
        }
    }
}
