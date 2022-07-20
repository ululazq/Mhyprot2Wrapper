using System;
using System.Diagnostics;
using EventHook;
using Mhyprot2Wrapper;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = @"
Max ASPD Recomendation
Destroyer = 20500
Witch = 19900
Rogue = 16500
Engineer = 16500
Swordman = 20500

Max MSPD Recomendation
All Classes = 720
";
            int aspd;
            float mspd;
            Console.WriteLine(text);
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
                        Hiddencp(aspd, mspd);
                    }
                    else if(e.KeyData.Keyname.Equals("D9") && e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Hiddencp(10000, 500);
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
            library.Write<int>(aspd, "ProjectN-Win64-Shipping.exe+063818E8,0,20,804");
            library.Write<float>(mspd, "ProjectN-Win64-Shipping.exe+063818E8,0,A0,288,18C");
            //Read from memory
            var current_attack_speed = library.Read<int>("ProjectN-Win64-Shipping.exe+063818E8,0,20,804");
            var current_movement_speed = library.Read<float>("ProjectN-Win64-Shipping.exe+063818E8,0,A0,288,18C");
            Console.WriteLine("Attackspeed : " + current_attack_speed);
            Console.WriteLine("Movespeed : " + current_movement_speed);
            library.CloseDriver();
        }
    }
}