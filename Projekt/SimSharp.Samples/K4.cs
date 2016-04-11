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
        static IEnumerable<Event> AProcess(Environment env)
        {
            Console.WriteLine("The time is {0}", env.NowD);
            yield return env.TimeoutD(3.0);
            Console.WriteLine("The time is {0}", env.NowD);
            yield return env.TimeoutD(3.0);
            Console.WriteLine("The time is {0}", env.NowD);
        }

        static void Main(string[] args)
        {
            var env = new Environment();
            env.Process(AProcess(env));
            env.Run();
        }



        public void Simulate(int rseed = 41) {
     
    }
  }
}
