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
            float td;
            Console.Write("TD : ");
            td = float.Parse(Console.ReadLine());
            Hiddencp(td,100);
            using (var eventHookFactory = new EventHookFactory())
            {
                var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
                keyboardWatcher.Start();
                keyboardWatcher.OnKeyInput += (s, e) =>
                {
                    if (e.KeyData.Keyname.Equals("D0") && e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Hiddencp(td,100);
                    }
                    else if(e.KeyData.Keyname.Equals("D9") && e.KeyData.EventType.Equals(KeyEvent.down))
                    {
                        Hiddencp(1,1);
                    }
                };
                var mouseWatcher = eventHookFactory.GetMouseWatcher();
                mouseWatcher.Start();
                mouseWatcher.OnMouseInput += (s, e) =>
                {
                    if (e.Message.ToString().Equals("WM_XBUTTONDOWN"))
                    {
                        Hiddencp(td,100);
                    }
                    else if (e.Message.ToString().Equals("WM_RBUTTONDOWN"))
                    {
                        Hiddencp(1,1);
                    }
                    //Console.WriteLine(string.Format("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y));
                };
                //waiting here to keep this thread running           
                Console.Read();
                //stop watching
                keyboardWatcher.Stop();
                mouseWatcher.Stop();
            }
        }

        static void Hiddencp(float td,int jump)
        {
            Library library = new Library();
            library.OpenProcess((uint)Process.GetProcessesByName("ProjectN-Win64-Shipping")[0].Id);
            //Write to memory
            library.Write<float>(td,"ProjectN-Win64-Shipping.exe+0664C8E8,0,20,98");
            library.Write<int>(jump, "ProjectN-Win64-Shipping.exe+0664C8E8,0,20,344");
            //Read from memory
            var current_td = library.Read<float>("ProjectN-Win64-Shipping.exe+0664C8E8,0,20,98");
            var current_jump = library.Read<int>("ProjectN-Win64-Shipping.exe+0664C8E8,0,20,344");
            Console.WriteLine("TD : " + current_td);
            Console.WriteLine("Jump : " + current_jump);
            library.CloseDriver();
        }
    }
}