#region License Information
/* SimSharp - A .NET port of SimPy, discrete event simulation framework
Copyright (C) 2016  Heuristic and Evolutionary Algorithms Laboratory (HEAL)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
#endregion

using System;
using System.Collections.Generic;

namespace SimSharp.Samples {
  public class K4 {
        TimeSpan ARRIVAL_TIME = TimeSpan.FromSeconds(360);
        TimeSpan PROCESSING_TIME = TimeSpan.FromSeconds(30);
        TimeSpan SIMULATION_TIME = TimeSpan.FromHours(10000);
        ContinuousStatistics statistics;

        IEnumerable<Event> Patient(Environment env)
        {
            var triage = new Resource(env, capacity: 1);
            while (true)
            {
                Console.WriteLine("Patient erreicht Krankenhaus");
                yield return env.TimeoutExponential(ARRIVAL_TIME);
                env.Process(triagierung(env, triage));
            }
        }

        IEnumerable<Event> triagierung(Environment env, Resource server)
        {
            using (var s = server.Request())
            {
                yield return s;
                Console.WriteLine("Patient wird triagiert");
                statistics.Update(server.InUse);
                yield return env.TimeoutExponential(PROCESSING_TIME);
            }
            statistics.Update(server.InUse);
        }

        void RunSimulation()
        {
            var env = new Environment(randomSeed: 42);
            statistics = new ContinuousStatistics(env);
            env.Process(Patient(env));
            env.Run(SIMULATION_TIME);
        }

    }
}
