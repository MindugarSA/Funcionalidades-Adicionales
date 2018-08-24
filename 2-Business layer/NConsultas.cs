using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FuncionalidadesAdicionales._1_Data_Layer;

namespace FuncionalidadesAdicionales._2_Business_layer
{
    class NConsultas
    {
        public static SAPbobsCOM.Recordset ConsultarArchivoImagenArticuloBEAS(string ItemCode)
        {
            DConsultas Obj = new DConsultas();
            return Obj.ConsultarArchivoImagenArticuloBEAS(ItemCode);
        }
    }
}
