using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SAPbobsCOM;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class FuncionesUDO
    {
        //Constructor Vacio
        public FuncionesUDO()
        {
        }

        public static int GetNextCode(string UDO_Name)
        {
            int nProx = 0;
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;
            try
            {
                //get company service
                if (!SBO_Company.Connected)
                    Conexion.Conectar_Aplicacion();

                SAPbobsCOM.Recordset oRecorset = (SAPbobsCOM.Recordset)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string sql = "select isnull(max(CAST(Code as int)),0)+1 as Proximo from [@" + UDO_Name + "]";
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
                
                string sql = "select * from ["+ Table + "] " + WhereCondition;
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
            catch (Exception){}
            return rpta;
        }

        public static string InsertRecord(string UDO_Name, Object Objeto, string UDO_Child, List<Object> DetalleObjeto)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;
            SAPbobsCOM.GeneralService oGeneralService;
            SAPbobsCOM.GeneralData oGeneralData;
            SAPbobsCOM.GeneralData oChild;
            SAPbobsCOM.GeneralDataCollection oChildren;
            //SAPbobsCOM.GeneralDataParams oGeneralParams;
            SAPbobsCOM.CompanyService oCompService;
            string rpta = "N";

            try
            {
                //get company service
                if (!SBO_Company.Connected)
                    Conexion.Conectar_Aplicacion();

                oCompService = SBO_Company.GetCompanyService();

                //SBO_Company.StartTransaction();

                oGeneralService = oCompService.GetGeneralService(UDO_Name);

                oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                //Setting Data to Master Data Table Fields
                oGeneralData.SetProperty("Code", FuncionesUDO.GetNextCode(UDO_Name).ToString());
                //Recorrer el Objeto y tomar Nombre y Valor de propiedades y asignarlas 
                foreach (PropertyInfo propiedad in Objeto.GetType().GetProperties())
                {
                    try
                    {
                        string tipoPropiedad = propiedad.PropertyType.Name;
                        string NombrePropiedad = propiedad.Name;
                        var valorPropiedad = propiedad.GetValue(Objeto, null);
                        oGeneralData.SetProperty(NombrePropiedad, valorPropiedad);
                    }
                    catch (Exception) { }
                }

                //  Handle child rows
                if (DetalleObjeto.Any())
                {
                    oChildren = oGeneralData.Child(UDO_Child);
                    foreach (Object det in DetalleObjeto)
                    {
                        // Create data for rows in the child table
                        oChild = oChildren.Add();
                        foreach (PropertyInfo dPropiedad in det.GetType().GetProperties())
                        {
                            try
                            {
                                string tipoPropiedad = dPropiedad.PropertyType.Name;
                                string NombrePropiedad = dPropiedad.Name;
                                var valorPropiedad = dPropiedad.GetValue(det, null);
                                oChild.SetProperty(NombrePropiedad, valorPropiedad);
                            }
                            catch (Exception) { }
                        }
                    }
                }
                //if(!DT_CHILD.IsEmpty)
                //{
                //    oChildren = oGeneralData.Child("SM_MOR1");
                //    for (int i = 0; i <= DT_CHILD.Rows.Count - 1; i++)
                //    {
                //    // Create data for rows in the child table
                //        oChild = oChildren.Add();
                //        for (int j = 0; j <= DT_CHILD.Columns.Count - 1; j++)
                //        {
                //            string nombreColumna = DT_CHILD.Columns.Item(j).Name;
                //            var valorColumna = DT_CHILD.GetValue(j, i);
                //            oChild.SetProperty(nombreColumna, valorColumna);
                //        }
                //    }
                //}

                //Attempt to Add the Record
                oGeneralService.Add(oGeneralData);
                rpta = "S";
            }
            catch (Exception) { }
            finally
            {
                if (SBO_Company.InTransaction)
                {
                    SBO_Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                }
            }
            return rpta;

            //try
            //{
            //    Type c_typo =  Objeto.GetType();
            //    PropertyInfo[] c_propiedades = c_typo.GetProperties();
            //    //var valor = c_propiedades.GetValue(Objeto);
            //    foreach (PropertyInfo propiedad in Objeto.GetType().GetProperties())
            //    {
            //        string tipoPropiedad = propiedad.PropertyType.Name;
            //        string NombrePropiedad = propiedad.Name;
            //        var valorPropiedad = propiedad.GetValue(Objeto, null);
            //    }

            //}
            //catch (Exception){}

        }

        public static string UpdateRecordHead(string UDO_Name, Object Objeto, string CodigoObj)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService sCmp = null;
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            sCmp = SBO_Company.GetCompanyService();
            string rpta = "N";

            //  This function updates only parent record.
            //  Child records remain as is.
            try
            {
                // Get a handle UDO
                oGeneralService = sCmp.GetGeneralService(UDO_Name);
                // Get UDO record
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("Code", CodigoObj);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                // Update UDO record
                foreach (PropertyInfo propiedad in Objeto.GetType().GetProperties())
                {
                    try
                    {
                        string tipoPropiedad = propiedad.PropertyType.Name;
                        string NombrePropiedad = propiedad.Name;
                        var valorPropiedad = propiedad.GetValue(Objeto, null);
                        oGeneralData.SetProperty(NombrePropiedad, valorPropiedad);
                    }
                    catch (Exception) { }
                }
                oGeneralService.Update(oGeneralData);
                rpta = "S";
            }
            catch (Exception) { }
            return rpta;
        }

        public static string DeleteRecord(string UDO_Name, string CodigoObj)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService sCmp = null;
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            sCmp = SBO_Company.GetCompanyService();
            string rpta = "N";
            try
            {
                // Get a handle to the SM_MOR UDO
                oGeneralService = sCmp.GetGeneralService(UDO_Name);

                // Delete UDO record
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("Code", CodigoObj);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralService.Delete(oGeneralParams);
            }
            catch (Exception){}
            return rpta;
        }

        public void CreateUDO(string tableName, SAPbobsCOM.BoUDOObjType objtype)
        {
            SAPbobsCOM.UserObjectsMD oUdtMD = null/* TODO Change to default(_) if this is not a reference type */; // ‘
            string errmsg = "";
            try
            {
                SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

                oUdtMD = (SAPbobsCOM.UserObjectsMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
                if (oUdtMD.GetByKey(tableName) == false)
                {
                    oUdtMD.Code = tableName;
                    oUdtMD.Name = tableName;
                    oUdtMD.TableName = tableName;

                    oUdtMD.ObjectType = objtype;

                    oUdtMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.CanClose = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                    oUdtMD.CanLog = SAPbobsCOM.BoYesNoEnum.tYES;
                    oUdtMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tYES;
                    oUdtMD.EnableEnhancedForm = SAPbobsCOM.BoYesNoEnum.tYES;
                    oUdtMD.MenuItem = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.UseUniqueFormType = SAPbobsCOM.BoYesNoEnum.tNO;
                    oUdtMD.Position = 1;
                    oUdtMD.FatherMenuID = 2048;
                    oUdtMD.LogTableName = "AREBATEMASTER";
                    if (objtype == SAPbobsCOM.BoUDOObjType.boud_MasterData)
                    {
                        oUdtMD.FormColumns.FormColumnAlias = "Code";
                        oUdtMD.FormColumns.Add();
                    }
                    else
                    {
                        oUdtMD.FormColumns.FormColumnAlias = "DocEntry";
                        oUdtMD.FormColumns.Add();
                    }

                    int lRetCode;
                    lRetCode = oUdtMD.Add();

                    if ((lRetCode != 0))
                    {
                        if ((lRetCode == -2035))
                            errmsg = "-2035";
                        errmsg = SBO_Company.GetLastErrorDescription();
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdtMD);
                    oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
                    GC.Collect();
                    errmsg = "";
                }
                else
                    errmsg = "";

            }
            catch (Exception)
            {
            }
        }

        public bool CheckUDOExists(string UDOName)
        {
            SAPbobsCOM.Company SBO_Company = Conexion.oCompany;

            SAPbobsCOM.UserObjectsMD oUdtMD = null/* TODO Change to default(_) if this is not a reference type */;
            bool ret = false;
            try
            {
                oUdtMD = (SAPbobsCOM.UserObjectsMD)SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);

                if (oUdtMD.GetByKey(UDOName))
                    ret = true;
                else
                    ret = false;
            }
            catch (Exception)
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


    }
}
