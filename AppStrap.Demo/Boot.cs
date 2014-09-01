﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStrap.Demo
{
    [Serializable]
    public class Boot : IAppStrapBoot
    {

        public void Start()
        {
            Utils.Scheduler sc = new Utils.Scheduler();
            sc.Start(TimeSpan.FromSeconds(1), () =>
            {
                Console.WriteLine("Demo Said : I can fly!");
                //throw new Exception("Exception Test");
            });
            
        }

        public void Stop()
        {
            Console.WriteLine("Demo Said : I am stop.");
        }
    }
}
