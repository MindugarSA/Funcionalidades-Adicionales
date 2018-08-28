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

        public string CreateUDT(string tableName, string tableDesc, SAPbobsCOM.BoUTBTableType tableType)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            //get company service
            if (!SBO_Company.Connected)
                Conexion.Conectar_Aplicacion();

            SAPbobsCOM.UserTablesMD oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
            try
            {
                oUdtMD = (SAPbobsCOM.UserTablesMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                if (oUdtMD.GetByKey(tableName) == false)
                {
                    oUdtMD.TableName = tableName;
                    oUdtMD.TableDescription = tableDesc;
                    oUdtMD.TableType = tableType;
                    int lRetCode;
                    lRetCode = oUdtMD.Add();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdtMD);
                    oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
                    GC.Collect();

                    if ((lRetCode != 0))
                    {
                        if ((lRetCode == -2035))
                            return "-2035";
                        return SBO_Company.GetLastErrorDescription();
                    }

                    return "";
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateUDF(string tableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes fieldType, int Size, string LinkTab, SAPbobsCOM.BoFldSubTypes SubType = SAPbobsCOM.BoFldSubTypes.st_None)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            //get company service
            if (!SBO_Company.Connected)
                Conexion.Conectar_Aplicacion();

            try
            {
                SAPbobsCOM.UserFieldsMD oUserFieldsMD;
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = tableName;
                oUserFieldsMD.Name = fieldName;
                oUserFieldsMD.Description = desc;
                oUserFieldsMD.Type = fieldType;
                if (Size != 0)
                    oUserFieldsMD.EditSize = Size;

                oUserFieldsMD.SubType = SubType;
                int lRetCode;
                lRetCode = oUserFieldsMD.Add();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                GC.Collect();
                oUserFieldsMD = null/* TODO Change to default(_) if this is not a reference type */;

                if (lRetCode != 0)
                {
                    if ((lRetCode == -2035 | lRetCode == -1120))
                        return System.Convert.ToString(lRetCode);
                    return SBO_Company.GetLastErrorDescription();
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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

        public bool CheckTableExists(string TableName)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            SAPbobsCOM.UserTablesMD oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
            bool ret = false;
            try
            {
                TableName = TableName.Replace("@", "");
                oUdtMD = (SAPbobsCOM.UserTablesMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

                if (oUdtMD.GetByKey(TableName))
                    ret = true;
                else
                    ret = false;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdtMD);
                oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
                GC.Collect();
            }

            return ret;
        }

        public bool CheckFieldExists(string TableName, string FieldName)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            SAPbobsCOM.UserFieldsMD oUserFieldsMD = null/* TODO Change to default(_) if this is not a reference type */;
            bool ret = false;
            try
            {
                FieldName = FieldName.Replace("U_", "");
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                int FieldID = getFieldidByName(TableName, FieldName);
                // ‘TableName = TableName.Replace(“@”, “”)
                if (oUserFieldsMD.GetByKey(TableName, FieldID))
                    ret = true;
                else
                    ret = false;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null/* TODO Change to default(_) if this is not a reference type */;
                GC.Collect();
            }

            return ret;
        }

        private int getFieldidByName(string TableName, string FieldName)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;
            int index = -1;
            SAPbobsCOM.Recordset ors;
            ors = (SAPbobsCOM.Recordset)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                if (SBO_Company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                    ors.DoQuery("select “”FieldID”” from “”CUFD”” where “”TableID”” = ‘" + TableName + "‘ and “”AliasID”” = ‘" + FieldName + "‘;");
                else
                    ors.DoQuery("select FieldID from CUFD where TableID = ‘" + TableName + "‘ and AliasID = ‘" + FieldName + "‘");

                if (!ors.EoF)
                    index = Convert.ToInt32( ors.Fields.Item("FieldID").Value);
            }
            catch (Exception)
            {
                return default(Int32);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ors);
                ors = null/* TODO Change to default(_) if this is not a reference type */;
                GC.Collect();
            }

            return index;
        }
    }
}
