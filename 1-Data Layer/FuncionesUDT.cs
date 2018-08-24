using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class FuncionesUDT
    {

        public static int GetNextCode(string UDT_Name)
        {
            int nProx = 0;
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;
            try
            {
                //get company service
                if (!SBO_Company.Connected)
                    Conexion.Conectar_Aplicacion();

                SAPbobsCOM.Recordset oRecorset = (SAPbobsCOM.Recordset)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string sql = "select isnull(max(CAST(Code as int)),0)+1 as Proximo from [@" + UDT_Name + "]";
                oRecorset.DoQuery(sql);
                nProx = (int)oRecorset.Fields.Item("Proximo").Value;
            }
            catch (Exception) { }

            return nProx;
        }

        public static string LoadObjectInfoFromRecordset(ref object Objeto, string Table, string WhereCondition)
        {
            string rpta = "N";
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;
            try
            {
                //get company service
                if (!SBO_Company.Connected)
                    Conexion.Conectar_Aplicacion();

                SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string sql = "select * from [" + Table + "] " + WhereCondition;
                oRecordSet.DoQuery(sql);

                if (oRecordSet.RecordCount > 0)
                {
                    rpta = "S";
                    oRecordSet.MoveFirst();
                    foreach (PropertyInfo propiedad in Objeto.GetType().GetProperties())
                    {
                        try
                        {
                            string tipoPropiedad = propiedad.PropertyType.Name;
                            string NombrePropiedad = propiedad.Name;
                            object valorPropiedad = propiedad.GetValue(Objeto, null);
                            propiedad.SetValue(Objeto, Convert.ChangeType(oRecordSet.Fields.Item(NombrePropiedad).Value, propiedad.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            rpta = "N";
                        }
                    }
                }
            }
            catch (Exception) { }
            return rpta;
        }

    }
}
