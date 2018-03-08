using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Keyboard
{
    class KeysAdresses
    {
        public static void assignKeys(Dictionary<string, int> DButton)
        {
            
            //Functional Keys
            DButton.Add("F1", 0x70);
            DButton.Add("F2", 0x71);
            DButton.Add("F3", 0x72);
            DButton.Add("F4", 0x73);
            DButton.Add("F5", 0x74);
            DButton.Add("F6", 0x75);
            DButton.Add("F7", 0x76);
            DButton.Add("F8", 0x77);
            DButton.Add("F9", 0x78);
            DButton.Add("F10", 0x79);
            DButton.Add("F11", 0x7A);
            DButton.Add("F12", 0x7B);
            DButton.Add("SHIFT", 0x10);
            DButton.Add(" ", 0x20);
            DButton.Add("ENTER", 0x0D);
            DButton.Add("'", 0xDE);
            DButton.Add("\"", 0x22);
            DButton.Add(";", 0xBA);


            //Numbers
            DButton.Add("0", 0x30);
            DButton.Add("1", 0x31);
            DButton.Add("2", 0x32);
            DButton.Add("3", 0x33);
            DButton.Add("4", 0x34);
            DButton.Add("5", 0x35);
            DButton.Add("6", 0x36);
            DButton.Add("7", 0x37);
            DButton.Add("8", 0x38);
            DButton.Add("9", 0x39);

            //Letters
            DButton.Add("A", 0x41);
            DButton.Add("B", 0x42);
            DButton.Add("C", 0x43);
            DButton.Add("D", 0x44);
            DButton.Add("E", 0x45);
            DButton.Add("F", 0x46);
            DButton.Add("G", 0x47);
            DButton.Add("H", 0x48);
            DButton.Add("I", 0x49);
            DButton.Add("J", 0x4A);
            DButton.Add("K", 0x4B);
            DButton.Add("L", 0x4C);
            DButton.Add("M", 0x4D);
            DButton.Add("N", 0x4E);
            DButton.Add("O", 0x4F);
            DButton.Add("P", 0x50);
            DButton.Add("Q", 0x51);
            DButton.Add("R", 0x52);
            DButton.Add("S", 0x53);
            DButton.Add("T", 0x54);
            DButton.Add("U", 0x55);
            DButton.Add("V", 0x56);
            DButton.Add("W", 0x57);
            DButton.Add("X", 0x58);
            DButton.Add("Y", 0x59);
            DButton.Add("Z", 0x5A);

            //Arrows
            DButton.Add("LEFT", 0x25);
            DButton.Add("RIGHT", 0x26);
            DButton.Add("UP", 0x27);
            DButton.Add("DOWN", 0x28);


    

        }
    }
}
