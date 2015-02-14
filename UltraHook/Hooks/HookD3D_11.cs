using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
    class HookD3D_11 : ID3DControl
    {
        public string Name { get; set; }

        public HookD3D_11()
        {
            Name = "Direct3D 11";
        }

        public void Hook()
        {
            Core.Log("Not implemented hook: " + Name);
        }
    }
}
