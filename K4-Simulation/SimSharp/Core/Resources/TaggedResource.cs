using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimSharp
{
    public class TaggedResource : Resource
    {
        protected List<bool> tags { get; private set; }

        public TaggedResource(Environment environment, int capacity = 1) : base(environment, capacity)
        {
           tags = new List<bool>(capacity);
           for(int i=0;i<tags.Capacity;i++)
            {
                tags[i] = false;
            }
        }

        public override Request Request()
        {
            var request = new Request(Environment, TriggerRelease, DisposeCallback);
            RequestQueue.AddLast(request);
            TriggerRequest();
            return request;
        }

    }
}
