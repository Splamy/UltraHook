using System;
using System.Collections.Generic;
using System.Text;

namespace UltraHook
{
    public interface ID3DControl
    {
        string Name { get; set; }

        void Hook();
    }
}
