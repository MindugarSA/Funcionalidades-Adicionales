using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FuncionalidadesAdicionales._1_Data_Layer;

namespace FuncionalidadesAdicionales._2_Business_layer
{
    class NTablaAutoriza
    {
        public static SAPbouiCOM.DataTable BuscarDatosAutorizacion(SAPbouiCOM.DataTable DT_SQL,string ObjType, string sDocNum)
        {
            DTablaAutoriza Obj = new DTablaAutoriza();
            return Obj.BuscarDatosAutorizacion(DT_SQL, ObjType, sDocNum);
        }

        public static string EliminarDatosAutorizacion(string sCodeAutoriza)
        {
            DTablaAutoriza Obj = new DTablaAutoriza();
            return Obj.EliminarDatosAutorizacion(sCodeAutoriza);
        }
        
        public static string InsertarDatosAutorizacion(string U_ObjType,
                                string U_DocEntry,
                                string U_DocNum,
                                string U_Approved,
                                string U_UserPrevDoc,
                                string U_UserSing,
                                string U_CreateDate)
        {
            DTablaAutoriza Obj = new DTablaAutoriza();
            Obj.U_ObjType = U_ObjType;
            Obj.U_DocEntry = U_DocEntry;
            Obj.U_DocNum = U_DocNum;
            Obj.U_Approved = U_Approved;
            Obj.U_UserPrevDoc = U_UserPrevDoc;
            Obj.U_UserSing = U_UserSing;
            Obj.U_CreateDate = U_CreateDate;

            List < Object > DetallesAutoriza = new List<Object>();

            return Obj.InsertarDatosAutorizacion(Obj, DetallesAutoriza);
        }


    }
}
