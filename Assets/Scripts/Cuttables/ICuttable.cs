using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidlandFarm.Scripts.Cuttables
{
    public interface ICuttable
    {
        void Cut(ICutter cutter);
    }
}
