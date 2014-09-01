using System;

namespace AppStrap.Demo
{
    [Serializable]
    public class Boot : IAppStrapBoot
    {

        public void Start()
        {
            Console.WriteLine("Demo Said : I can fly!");
            //throw new Exception("Exception Test");
        }

        public void Stop()
        {
            Console.WriteLine("Demo Said : Stop.");
        }
    }
}
