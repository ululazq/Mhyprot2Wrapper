using System;
using System.Diagnostics;
using System.Threading;
using EventHook;
using Mhyprot2Wrapper;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int aspd;
            float mspd; 
            Console.Write("ASPD : ");
            aspd = int.Parse(Console.ReadLine());
            Console.Write("MSPD : ");
            mspd = float.Parse(Console.ReadLine());
            Hiddencp(aspd, mspd);
            using (var eventHookFactory = new EventHookFactory())
            {
                var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
                keyboardWatcher.Start();
                keyboardWatcher.OnKeyInput += (s, e) =>
                {
                    if (e.KeyData.Keyname.Equals("D0") && e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Hiddencp(aspd,mspd);
                    }
                };
                //waiting here to keep this thread running           
                Console.Read();
                //stop watching
                keyboardWatcher.Stop();
            }
        }

        static void Hiddencp(int aspd,float mspd)
        {
            Library library = new Library();
            library.OpenProcess((uint)Process.GetProcessesByName("ProjectN-Win64-Shipping")[0].Id);
            //Write to memory
            library.Write<int>(aspd, "ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
            library.Write<float>(mspd, "ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
            //Read from memory
            var current_attack_speed = library.Read<int>("ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
            var current_movement_speed = library.Read<float>("ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
            Console.WriteLine("Attackspeed : " + current_attack_speed);
            Console.WriteLine("Movespeed : " + current_movement_speed);
            library.CloseDriver();
        }
    }
}