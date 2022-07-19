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
                keyboardWatcher.Start();
                keyboardWatcher.OnKeyInput += (s, e) =>
                {
                    if (e.KeyData.Keyname.Equals("D0")&&e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Library library = new Library();
                        library.OpenProcess((uint)Process.GetProcessesByName("ProjectN-Win64-Shipping")[0].Id);
                        //Write to memory
                        library.Write<float>(720, "ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
                        library.Write<int>(16000, "ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
                        //Read from memory
                        var current_movement_speed = library.Read<float>("ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
                        var current_attack_speed = library.Read<int>("ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
                        Console.WriteLine("Movespeed : " + current_movement_speed);
                        Console.WriteLine("Attackspeed : " + current_attack_speed);
                        library.CloseDriver();
                    }
                };
                //waiting here to keep this thread running           
                Console.Read();
                //stop watching
                keyboardWatcher.Stop();
            }
        }
    }
}