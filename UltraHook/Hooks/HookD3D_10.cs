using System;
using System.Collections.Generic;
using System.Text;



using EasyHook;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;

namespace UltraHook
{
    class HookD3D_10 : ID3DControl
    {
		public override string Name { get { return "Direct3D 10"; } protected set { } }

        public HookD3D_10()
        {
            
        }

		public override void Hook()
        {
            Core.Log("Not implemented hook: " + Name);
        }
    }
}
