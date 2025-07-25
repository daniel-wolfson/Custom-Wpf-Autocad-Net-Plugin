﻿using System;
using System.ServiceProcess;

namespace ID.WS.StateService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new IDWSStateService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
