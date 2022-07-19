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
            using (var eventHookFactory = new EventHookFactory())
            {
                var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();

                string anu = "D0";
                keyboardWatcher.Start();
                keyboardWatcher.OnKeyInput += (s, e) =>
                {
                    //Console.WriteLine(string.Format("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname));
                    //balik:
                    if (e.KeyData.Keyname.Equals(anu)&&e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Library library = new Library();
                        library.OpenProcess((uint)Process.GetProcessesByName("ProjectN-Win64-Shipping")[0].Id);
                        //Write to memory
                        library.Write<float>(720, "ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
                        library.Write<int>(16000, "ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
                        //library.Write<int>(100, "ProjectN-Win64-Shipping.exe+637B7C0,188,38,0,30,228,280,344");
                        //Read from memory
                        var current_movement_speed = library.Read<float>("ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
                        var current_attack_speed = library.Read<int>("ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
                        //var current_attack_speed2 = library.Read<int>("ProjectN-Win64-Shipping.exe+637B7C0,188,38,0,30,228,280,344");
                        Console.WriteLine("Movespeed : " + current_movement_speed);
                        Console.WriteLine("Attackspeed : " + current_attack_speed);
                        //Console.WriteLine("Attackspeed : " + current_attack_speed2);
                        //Console.ReadKey();
                        //Thread.Sleep(10000);
                        library.CloseDriver();
                    }
                };
                //waiting here to keep this thread running           
                Console.Read();
                //goto balik;
                //stop watching
                keyboardWatcher.Stop();
            }
        }
    }
}