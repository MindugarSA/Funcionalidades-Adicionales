using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class DTablaAutoriza
    {
        string _U_ObjType;
        string _U_DocEntry;
        string _U_DocNum;
        string _U_Approved;
        string _U_UserPrevDoc;
        string _U_UserSing;
        string _U_CreateDate;

        public string U_ObjType
        {
            get { return _U_ObjType; }
            set { _U_ObjType = value; }
        }
        public string U_DocEntry
        {
            get { return _U_DocEntry; }
            set { _U_DocEntry = value; }
        }
        public string U_DocNum
        {
            get { return _U_DocNum; }
            set { _U_DocNum = value; }
        }
        public string U_Approved
        {
            get { return _U_Approved; }
            set { _U_Approved = value; }
        }
        public string U_UserPrevDoc
        {
            get { return _U_UserPrevDoc; }
            set { _U_UserPrevDoc = value; }
        }
        public string U_UserSing
        {
            get { return _U_UserSing; }
            set { _U_UserSing = value; }
        }
        public string U_CreateDate
        {
            get { return _U_CreateDate; }
            set { _U_CreateDate = value; }
        }

        public DTablaAutoriza()
        {
        }

        public DTablaAutoriza(string U_ObjType,
                                string U_DocEntry,
                                string U_DocNum,
                                string U_Approved,
                                string U_UserPrevDoc,
                                string U_UserSing,
                                string U_CreateDate)
        {
            this.U_ObjType = U_ObjType;
            this.U_DocEntry = U_DocEntry;
            this.U_DocNum = U_DocNum;
            this.U_Approved = U_Approved;
            this.U_UserPrevDoc = U_UserPrevDoc;
            this.U_UserSing = U_UserSing;
            this.U_CreateDate = U_CreateDate;
        }

        public SAPbouiCOM.DataTable BuscarDatosAutorizacion(SAPbouiCOM.DataTable DT_SQL, string ObjType, string sDocNum)
        {
            try
            {
                string sql = "SELECT * FROM [@ZAUTORI] WHERE U_ObjType = '" + ObjType  + "' AND U_DocNum = ISNULL(" + sDocNum + ",0) ORDER BY DocEntry";
                DT_SQL.ExecuteQuery(sql);
            }
            catch (Exception){}
            
            return DT_SQL;
        }

        public string InsertarDatosAutorizacion(DTablaAutoriza DatosAutoriza, List<Object> DetallesAutoriza)
        {

            string rpta = "N";

            try
            {
                rpta = FuncionesUDO.InsertRecord("ZAUTORI", DatosAutoriza, "", DetallesAutoriza);
            }
            catch (Exception) { }

            return rpta;
        }

        public string EliminarDatosAutorizacion(string sCodeAutoriza)
        {

            string rpta = "N";

            try
            {
                rpta = FuncionesUDO.DeleteRecord("ZAUTORI", sCodeAutoriza);
            }
            catch (Exception) { }

            return rpta;
        }
    }
}
