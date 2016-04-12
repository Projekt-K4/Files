using System;
using System.Collections.Generic;

namespace SimSharp.Samples {
  public class K4 {
        TimeSpan ARRIVAL_TIME = TimeSpan.FromSeconds(360);
        TimeSpan PROCESSING_TIME = TimeSpan.FromSeconds(30);
        TimeSpan SIMULATION_TIME = TimeSpan.FromHours(1000);

        static IEnumerable<Event> Patient(Environment env)
        {
            var objects = new[] { "A", "B", "C", "D" };
            foreach (var obj in objects)
            {
                yield return env.TimeoutUniform(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
                Console.WriteLine("{0:mm:ss} {1}", env.Now, obj);
                eventLog.getLog().addLog(env.Now, "---", obj, "arrived");
                env.Process(ASubprocess(env, obj));
                eventLog.getLog().addLog(env.Now, "---", obj, "triagiert");
            }
        }
        static IEnumerable<Event> ASubprocess(Environment env, string pat)
        {
            eventLog.getLog().addLog(env.Now, "---", pat, "at triage");
            yield return env.TimeoutD(30.0);
            eventLog.getLog().addLog(env.Now, "---", pat, "get number");
        }




        public void RunSimulation()
        {
            var env = new Environment(randomSeed: 41,defaultStep: TimeSpan.FromMinutes(1));
            
            env.Process(Patient(env));
            env.RunD(20);



        }

    }
}
