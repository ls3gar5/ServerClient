using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropExamples
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IEventos
    {
        [DispId(1)]  void finalizo(bool lExito);
    }
}
