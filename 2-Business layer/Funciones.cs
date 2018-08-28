using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;
using System.Globalization;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._1_Data_Layer;
using System.Diagnostics;
using System.IO;

namespace FuncionalidadesAdicionales
{
    class Funciones
    {

        public Funciones()
        {
            Conexion.Conectar_Aplicacion();
        }
        
        public static SAPbouiCOM.SboGuiApi SboGuiApi = null;
        public static SAPbouiCOM.Application SBO_App = null;
        public static SAPbobsCOM.Company oCompany =  Program.oCompany;
        public static SAPbobsCOM.Recordset oRsSUers = null;
        public static SAPbobsCOM.SBObob oSBObob = null;
        public static SAPbouiCOM.DataTable oDataTable = null;
        public static SAPbouiCOM.UserDataSource oUserDataSource = null;

        public static bool ConsultaUsuario = false;
        public static bool Habilitado = false;

        public static void KillProcess(string ProcessName)
        {
            foreach (var p in System.Diagnostics.Process.GetProcessesByName(ProcessName))
            {
                p.Kill();
            }
            //foreach (Process proceso in Process.GetProcesses())
            //{
            //    if (proceso.ProcessName == ProcessName)
            //    {
            //        proceso.Kill();
            //    }

            //}
        }

        public static void Connect_Application(ref SAPbobsCOM.Company oCompany)
        {
            Conexion.Conectar_Aplicacion();
            Program.oCompany = Conexion.oCompany;
            oCompany = Conexion.oCompany;
        }

        public static bool ItemExists(SAPbouiCOM.Form oForm, string ItemUid)
        {
            try
            {
                SAPbouiCOM.Item oItem = oForm.Items.Item(ItemUid);
            }
            catch
            {
                // item does not exist  
                return false;
            }
            return true;

        }

        public static bool DataTableExists(SAPbouiCOM.Form oForm, string ItemUid)
        {
            try
            {
                SAPbouiCOM.DataTable oDataTable = oForm.DataSources.DataTables.Item(ItemUid);
            }
            catch
            {
                // item does not exist  
                return false;
            }
            return true;
        }

        public static bool UserDataSourceExists(SAPbouiCOM.Form oForm, string ItemUid)
        {
            try
            {
                SAPbouiCOM.UserDataSource oUDataSource = oForm.DataSources.UserDataSources.Item(ItemUid);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        //  EJECUTA UN ARCHIVO CONTENIDO EN LA RUTA INGRESADA COMO PARAMETRO
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static void Open_File(String Path)
        {

            ProcessStartInfo psi = new ProcessStartInfo();

            psi.UseShellExecute = true;

            psi.FileName = Path;

            try
            {
                Process.Start(psi);
            }
            catch (Exception)
            {
                Application.SBO_Application.MessageBox("Ruta de archivo Invalida");
            }
        }

        public static void Copy_File_to_Directoy(string FilePath, string DestinyPath)
        {

            try
            {
                if (Directory.Exists(System.IO.Path.GetDirectoryName(FilePath)))
                    if (Directory.Exists(System.IO.Path.GetDirectoryName(DestinyPath)))
                    {
                        string FileName = System.IO.Path.GetFileName(FilePath);
                        System.IO.File.Copy(FilePath, DestinyPath + @"\" + FileName, true);
                    }
                    else
                        Application.SBO_Application.MessageBox("No existe el directorio " + DestinyPath.Trim());
                else
                    Application.SBO_Application.MessageBox("No existe el directorio " + FilePath.Trim());
            }
            catch (Exception)
            {
                Application.SBO_Application.MessageBox("Error al Copiar el Anexo a " + DestinyPath.Trim());
            }

        }

        public static void Delete_File(string FilePath)
        {
            // Delete a file by using File class static method...
            if (System.IO.File.Exists(FilePath))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(FilePath);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public static void AgregarUDF_Salida_Inventario()
        {
            try
            {
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                String sql = @"SELECT *
                                FROM INFORMATION_SCHEMA.COLUMNS
                                WHERE COLUMN_NAME = 'U_Solicitud_Compra' AND TABLE_NAME = 'OIGE' ";
                rs.DoQuery(sql);
                // Si no esta creado el campo, se registra en la estructura de la BD
                if (rs.RecordCount == 0)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

                    Agregar_UDF("Solicitud_Compra", "Solicitud Compra Interna", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, 50, "OIGE");

                    //SAPbobsCOM.UserFieldsMD oUserFieldsMD = oCompany.GetBusinessObject(BoObjectTypes.oUserFields);

                    //oUserFieldsMD.Name = "Solicitud_Compra";
                    //oUserFieldsMD.Description = "Solicitud Compra Interna";
                    //oUserFieldsMD.Type = BoFieldTypes.db_Alpha;
                    //oUserFieldsMD.Size = 50;
                    //oUserFieldsMD.EditSize = 50;
                    //oUserFieldsMD.TableName = "OIGE";


                    //// Adding the Field to the Table
                    //try
                    //{
                    //    int lRetCode = oUserFieldsMD.Add();
                    //}
                    //catch (Exception)
                    //{ }
                    //finally
                    //{
                    //    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                    //    oUserFieldsMD = null;
                    //    GC.Collect();
                    // }
                }
                else
                { System.Runtime.InteropServices.Marshal.ReleaseComObject(rs); }

                SAPbobsCOM.Recordset rs2 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = @"SELECT *
                                FROM INFORMATION_SCHEMA.COLUMNS
                                WHERE COLUMN_NAME = 'U_RUT_Receptor' AND TABLE_NAME = 'OIGE' ";
                rs2.DoQuery(sql);
                // Si no esta creado el campo, se registra en la estructura de la BD
                if (rs2.RecordCount == 0)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rs2);

                    Agregar_UDF("Rut_Receptor", "Rut de Persona Recibe", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, 50, "OIGE");

                }
                else
                { System.Runtime.InteropServices.Marshal.ReleaseComObject(rs2); }

                SAPbobsCOM.Recordset rs3 = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = @"SELECT *
                                FROM INFORMATION_SCHEMA.COLUMNS
                                WHERE COLUMN_NAME = 'U_Nombre_Receptor' AND TABLE_NAME = 'OIGE' ";
                rs3.DoQuery(sql);
                // Si no esta creado el campo, se registra en la estructura de la BD
                if (rs3.RecordCount == 0)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rs3);

                    Agregar_UDF("Nombre_Receptor", "Nombre Persona Recibe", SAPbobsCOM.BoFieldTypes.db_Alpha, 150, 150, "OIGE");

                }
                else
                { System.Runtime.InteropServices.Marshal.ReleaseComObject(rs3); }

            }
            catch (Exception)
            { }


        }

       

        public static void Agregar_UDF(string Name, string Description, SAPbobsCOM.BoFieldTypes Type, int Size, int FieldSize, string TableName)
        {
            try
            {
                SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.Name = Name;
                oUserFieldsMD.Description = Description;
                oUserFieldsMD.Type = Type;
                oUserFieldsMD.Size = Size;
                oUserFieldsMD.EditSize = FieldSize;
                oUserFieldsMD.TableName = TableName;


                // Adding the Field to the Table
                try
                {
                    int lRetCode = oUserFieldsMD.Add();
                }
                catch (Exception)
                { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                    oUserFieldsMD = null;
                    GC.Collect();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void IdentificarAccesoUsuario(SAPbouiCOM.DataTable oDataTable)
        {
            string Acceso = "";
            if (!Conexion.oCompany.Connected)
                Conexion.Conectar_Aplicacion();
            string sAliasUsuActual = Convert.ToString(Conexion.oCompany.UserName);
            string sBDComercial = "SBO_COMERCIAL";
            //SAPbouiCOM.DataTable DT_SQL = new SAPbouiCOM.DataTable();

            string sPSql = "SELECT * FROM [" + sBDComercial + "].dbo.[OUSR] where USER_Code ='" + sAliasUsuActual + "'";

            try
            {
                oDataTable.ExecuteQuery(sPSql);
                Acceso = Convert.ToString(oDataTable.GetValue("U_Hab_Fec_Proy", 0));
                Habilitado = Acceso == "Si" ? true : false;
                ConsultaUsuario = true;
            }
            catch
            {
                // item does not exist  
                Habilitado = false;
            }

        }

        public static void CargarDatosPagos(SAPbouiCOM.Form oForm)
        {
            try
            {
                SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("8").Specific;
                SAPbouiCOM.DataTable oDataTable = null;
                try
                {
                    oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");
                }
                catch
                {
                    oDataTable = oForm.DataSources.DataTables.Add("DT_SQL");
                }


                string nDoc = oEdit.Value;

                string BD = Convert.ToString(oCompany.CompanyDB);

                string sql = BD + ".[dbo].[SP_Saldo_Pediente_Orden_Compra] " + nDoc;

                //            string sql = @"SELECT   SC.DocNum as 'N° Pedido'
                //		                            ,SC.CardCode as 'Codigo Proveedor'
                //		                            ,SC.CardName as 'Proveedor'
                //		                            ,SC.TaxDate as 'Emision Pedido'
                //		                            ,SC.DocTotal as 'Total Pedido'
                //		                            ,'$ ' + REPLACE(REPLACE(CONVERT(NVARCHAR, convert(money, SUM(ISNULL(PC.SumApplied,0))), 1),'.00',''),',','.')  as 'Total Cancelado'
                //		                            ,'$ ' + REPLACE(REPLACE(CONVERT(NVARCHAR, convert(money, (SC.DocTotal - SUM(ISNULL(PC.SumApplied,0)))), 1),'.00',''),',','.') as 'Saldo Pendiente'
                //                            FROM OPOR SC LEFT JOIN OPCH FC ON SC.DocNum = FC.U_MIN_Pedido AND FC.CANCELED != 'Y'
                //                            LEFT JOIN VPM2 PC ON FC.DocEntry = PC.DocEntry
                //                            LEFT JOIN OVPM CPC ON PC.DocNum = CPC.DocNum  
                //                            WHERE SC.DocNum = " + nDoc + @" AND  ISNULL(CPC.Canceled,'N') != 'Y'
                //                            GROUP BY SC.DocNum 
                //		                                ,SC.CardCode
                //		                                ,SC.CardName
                //		                                ,SC.TaxDate 
                //		                                ,SC.DocTotal
                //                            --HAVING SC.DocTotal > SUM(ISNULL(PC.SumApplied,0))
                //                            ORDER BY SC.DocNum";

                oDataTable.ExecuteQuery(sql);

                if (oDataTable.IsEmpty == false)
                {
                    if (Funciones.ItemExists(oForm, "Item_0") == true)
                    {
                        oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_TP");
                        oUserDataSource.ValueEx = oDataTable.GetValue(5, 0).ToString();
                        oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_SP");
                        oUserDataSource.ValueEx = oDataTable.GetValue(6, 0).ToString();

                        //oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific;
                        //sql = (string)oDataTable.GetValue(5, 0);
                        //oEdit.String = sql;
                        //oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_1").Specific;
                        //sql = (string)oDataTable.GetValue(6, 0);
                        //oEdit.String = sql;
                    }
                }
                else
                {
                    oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_TP");
                    oUserDataSource.ValueEx = "";
                    oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_SP");
                    oUserDataSource.ValueEx = "";

                    //oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific;
                    //oEdit.String = "";
                    //oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_1").Specific;
                    //oEdit.String = "";
                }
            }
            catch
            {
            }

        }
        public static string Current_User_Name()
        {
            string sNombreUsu = "";
            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.GetForm("169", 0); //Toma la Descripcion del Usuario Actual del Menu Principal
            SAPbouiCOM.StaticText oStatic = (SAPbouiCOM.StaticText)oForm.Items.Item("8").Specific;
            sNombreUsu = oStatic.Caption;
            return (string)sNombreUsu;
        }

        public static string FormatMoneyToString(double _double, SAPbobsCOM.Company oCompany, BoMoneyPrecisionTypes _Precision)
        {
            SBObob businessObject = (SBObob)oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
            Recordset recordset = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset = businessObject.Format_MoneyToString(_double, _Precision);
            return (string)recordset.Fields.Item(0).Value;
        }

        public static SAPbouiCOM.DataTable GetDataTableFromCLF(SAPbouiCOM.ItemEvent oEvent, SAPbouiCOM.Form oForm)
        {
            SAPbouiCOM.ChooseFromListEvent event2 = (SAPbouiCOM.ChooseFromListEvent)oEvent;
            string chooseFromListUID = event2.ChooseFromListUID;
            SAPbouiCOM.ChooseFromList list = oForm.ChooseFromLists.Item(chooseFromListUID);
            return event2.SelectedObjects;
        }

        public static double GetDoubleFromString(string _doublestring)
        {
            _doublestring = _doublestring.Trim().Substring(0, 1) == "." ? "0" + _doublestring : _doublestring;
            if (Program.oNumberFormatInfo.NumberDecimalSeparator == ",")
            {
                return double.Parse(_doublestring, CultureInfo.CurrentCulture);
            }
            return double.Parse(_doublestring, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public static string GetStringFromDouble(double _double)
        {
            try
            {
                return _double.ToString().Replace(",", ".");
            }
            catch (Exception)
            {
                return "0.00";
            }
        }

        public static string GetStringFromDoubleDecimal(double _double, int _decimal)
        {
            string str3 = "";
            try
            {
                str3 = _double.ToString().Replace(",", ".");
                if (str3.IndexOf(".") == -1)
                {
                    return str3;
                }
                return str3.Substring(0, (str3.IndexOf(".") + _decimal) + 1);
            }
            catch (Exception)
            {
                return "0.00";
            }
        }

        public static void Create_ContextMenu(String IDMenu, String Descripcion, int Position)
        {
            try
            {
                if (Application.SBO_Application.Menus.Exists(IDMenu) == true)
                    Application.SBO_Application.Menus.RemoveEx(IDMenu);

                SAPbouiCOM.MenuItem oMenuItem;
                SAPbouiCOM.Menus oMenus;

                SAPbouiCOM.MenuCreationParams oCreationPackage = null;
                oCreationPackage = (SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams));

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = IDMenu;
                oCreationPackage.String = Descripcion;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = Position;

                oMenuItem = Application.SBO_Application.Menus.Item("1280"); //Data
                oMenus = oMenuItem.SubMenus;
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception){}
        }

        public static void AddUserDataSource(SAPbouiCOM.Form oForm, string Nombre, SAPbouiCOM.BoDataType Tipo, int Longitud)
        {
            try
            {
                if (!UserDataSourceExists(oForm, Nombre))
                    oForm.DataSources.UserDataSources.Add(Nombre, Tipo, Longitud);
            }
            catch (Exception){}
        }

    }
}
