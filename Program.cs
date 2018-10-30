using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using SAPbobsCOM;
using System.Globalization;

using FuncionalidadesAdicionales._1_Data_Layer;
using FuncionalidadesAdicionales._2_Business_layer;
using FuncionalidadesAdicionales._3_Presentation_Layer.System_Forms;

namespace FuncionalidadesAdicionales
{
    class Program
    {
        //'Variables de Conexion a las BD DI API
        //'------------------------------------------------------------
        public static SAPbouiCOM.SboGuiApi SboGuiApi = null;
        public static SAPbouiCOM.Application SBO_App = null;
        public static SAPbobsCOM.Company oCompany = null;
        public static SAPbobsCOM.Recordset oRsSUers = null;
        public static SAPbobsCOM.SBObob oSBObob = null;
        public static SAPbouiCOM.DataTable oDataTable = null;
        public static SAPbouiCOM.UserDataSource oUserDataSource = null;
        public static bool ConsultaUsuario = Funciones.ConsultaUsuario;
        public static bool Habilitado = Funciones.Habilitado ;
        public static bool AbiertoDesdeEnlace = false;
        //Numero de Objetos a Bloquear o Habilitar Campos de Fechas en la ventanas de UDF en los documentos de Marketing
        public static int[] iFormularios = { 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

        public static string sBDComercial = "SBO_COMERCIAL";
        public static string sBDIndustrial = "SBO_INDUSTRIAL";
        public static string sBDMontaje = "SBO_MONTAJE";

        public static NumberFormatInfo oNumberFormatInfo = new NumberFormatInfo();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    oApp = new Application(args[0]);
                }
                Menu MyMenu = new Menu();
                MyMenu.AddMenuItems();
                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                // events handled by SBO_Application_MenuEvent 
                Application.SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
                // events handled by SBO_Application_ItemEvent
                Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
                // events handled by SBO_Application_FormDataEvent
                Application.SBO_Application.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormDataEvent);
                Application.SBO_Application.RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(SBO_Application_RightClickEvent);
                Conexion.Conectar_Aplicacion();
                oCompany = Conexion.oCompany;
                Funciones.AgregarUDF_Salida_Inventario();
                NVerificaAgregaUDO.VerificarCrearUDO();
                oApp.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    System.Windows.Forms.Application.ExitThread();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    System.Windows.Forms.Application.Exit();
                    //Conexion.Conectar_Aplicacion();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    System.Windows.Forms.Application.Exit();
                    System.Windows.Forms.Application.ExitThread();
                    break;
                default:
                    break;
            }
        }

        static void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            //------------------------------------------------------------------------------------------------------------------------------------------------
            //  ESTE EVENTO VERIFICA SI LA PANTALLA ACTIVA ES MODAL; SI ES ASI SE ANULA EL EVENTO MENU
            //------------------------------------------------------------------------------------------------------------------------------------------------
            if (NModal.esPantallaModal)
            {
                if (pVal.MenuUID == "773") //Menu Contextual "Pegar" Permitido
                    BubbleEvent = true;
                else
                    BubbleEvent = false;
            }

            // Boton CREAR Y BUSCAR EN PEDIDO COMPRA  ------------------------------------------------------------------------------------

            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.ActiveForm;

            switch (oForm.TypeEx)
            {
                case "142": //Ordenes de Compra
                    OrdenCompra.OrdenCompra_MenuEvent(ref pVal, out BubbleEvent);  
                    break;
                case "139": //Pedidos Clientes
                    if (pVal.BeforeAction == true)
                    {
                        switch (pVal.MenuUID)
                        {
                            case "Ver Imagen 1 Artículo BEAS":
                                AbrirArchivoImagenArticuloBEAS(oForm, "PicturName");
                                break;
                            case "Ver Imagen 2 Artículo BEAS":
                                AbrirArchivoImagenArticuloBEAS(oForm, "U_picture2");
                                break;
                            case "Ver Imagen 3 Artículo BEAS":
                                AbrirArchivoImagenArticuloBEAS(oForm, "U_picture3");
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.Form oForm = null;
            SAPbouiCOM.EditText oEdit = null;
            SAPbouiCOM.DataTable oDataTable = null;

            try
            {
                // ------------------------------------------------------------------------------------------------------------------------------------------------
                //   ESTOS EVENTO MANEJA LA CONDICION MODAL DE LAS PANTALLAS DONDE ReportType = "Modal"
                // ------------------------------------------------------------------------------------------------------------------------------------------------
                if (((pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_VISIBLE)
                            && (pVal.BeforeAction == false)))
                {
                    try
                    {
                        oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                        if ((oForm.ReportType == "Modal"))
                        {
                            NModal.esPantallaModal = true;
                            NModal.IDPantallaModal = pVal.FormUID;
                        }
                    }
                    catch (Exception) { }
                }

                if (NModal.esPantallaModal
                            && pVal.FormUID != NModal.IDPantallaModal
                            && NModal.IDPantallaModal.Trim().Length > 0
                            && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_GOT_FOCUS
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_RESIZE
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_VALIDATE
                            || pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK))
                {
                    try
                    {
                        //  Selecciona la pantalla modal
                        BubbleEvent = false;
                        oForm = Application.SBO_Application.Forms.Item(NModal.IDPantallaModal);
                        oForm.Select();
                    }
                    catch (Exception) { }
                }

                //  If the modal from is closed...
                if (((FormUID == NModal.IDPantallaModal)
                            && ((pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_CLOSE)
                            && NModal.esPantallaModal)))
                {
                    NModal.esPantallaModal = false;
                    NModal.IDPantallaModal = "";
                }

                // ------------------------------------------------------------------------------------------------------------------------------------------------


                //Ordenes de Compra 
                if (pVal.FormTypeEx == "142")
                {
                    OrdenCompra.OrdenCompra_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                }

                //Enviar Solicitud de Compra Interna a Salida de Mercancias (Exportar Lineas y datos del Documento Base)
                if (pVal.FormTypeEx == "1470000200" && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT) && (pVal.ItemUID == "10000329"))
                {
                    SolicitudCompra.SolicitudCompra_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                }

                int ObjN = Math.Abs(Convert.ToInt32(pVal.FormTypeEx));
                //Bloquear o Habilitar Campos de Fechas en la ventanas de UDF en los documentos de Marketing
                if ((pVal.FormTypeEx.Substring(0, 1) == "-") && (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) 
                    && pVal.BeforeAction == false)
                {
                    string[] U_Campos = { "U_MIN_FechaVenta", "U_MIN_FinMontaje", "U_MIN_FinDesarr", "U_MIN_FinProduccion", "U_MIN_IniDespacho", "U_MIN_IniMontaje", "U_MIN_FinMontaje", "U_MIN_FechaMulta" };

                    try
                    {
                        oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);

                        if (Funciones.ConsultaUsuario == false)
                        {
                            oDataTable = null;
                            try
                            {
                                oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");
                            }
                            catch
                            {
                                oDataTable = oForm.DataSources.DataTables.Add("DT_SQL");
                            }
                            Funciones.IdentificarAccesoUsuario(oDataTable);
                        }

                        foreach (string element in U_Campos)
                        {
                            if (Funciones.ItemExists(oForm, element))
                            {
                                oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(element).Specific;
                                oEdit.Item.Enabled = Funciones.Habilitado;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // throw;
                    }
                }
            }
            catch (Exception) { }
        }

        static void SBO_Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BOInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(BOInfo.FormUID);
            try
            {
                if ((BOInfo.FormTypeEx == "142"))
                {
                    OrdenCompra.OrdenCompra_FormDataEvent(ref BOInfo, out BubbleEvent);
                }

                if ((BOInfo.FormTypeEx == "141"))
                {
                    FacturaProveedores.FacturaProveedores_FormDataEvent(ref BOInfo, out BubbleEvent);
                }

                int ObjN = Convert.ToInt32(BOInfo.Type);
                //Bloquear o Habilitar Campos de Fechas en la ventanas de UDF en los documentos de Marketing segun iFormularios
                if ((BOInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD) && BOInfo.BeforeAction && Array.Exists(iFormularios, element => element == ObjN))
                {
                    string[] U_Campos = { "U_MIN_FechaVenta", "U_MIN_FinMontaje", "U_MIN_FinDesarr", "U_MIN_FinProduccion", "U_MIN_IniDespacho", "U_MIN_IniMontaje", "U_MIN_FinMontaje", "U_MIN_FechaMulta" };
                    try
                    {
                        oForm = Application.SBO_Application.Forms.GetForm("-" + BOInfo.FormTypeEx, 1);// Item(BOInfo.FormUID);
                        SAPbouiCOM.EditText oEdit;

                        if (Funciones.ConsultaUsuario == false)
                        {
                            SAPbouiCOM.DataTable oDataTable = null;
                            if (!Funciones.DataTableExists(oForm, "DT_SQL"))
                                oForm.DataSources.DataTables.Add("DT_SQL");
                            oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");

                            Funciones.IdentificarAccesoUsuario(oDataTable);
                        }

                        foreach (string element in U_Campos)
                        {
                            if (Funciones.ItemExists(oForm, element))
                            {
                                oEdit = (SAPbouiCOM.EditText)oForm.Items.Item(element).Specific;
                                if (oEdit.Item.Enabled == Funciones.Habilitado) // Verifica si ya estan condicionados los campos para no volver a ejecutar el proceso en la pantalla activa
                                    break;
                                oEdit.Item.Enabled = Funciones.Habilitado;
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        static void SBO_Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool BubbleEvent)
        {

            BubbleEvent = true;

            try
            {
                //SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.ActiveForm; eventInfo.FormUID
                SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(eventInfo.FormUID);

                switch (oForm.TypeEx)
                {
                    case "139": // Agrega Opciones al Menu Contextual en la pantalla Pedidos a Cliente
                        if (eventInfo.ItemUID == "38" && eventInfo.BeforeAction == true)
                        {
                            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
                            int nRow = eventInfo.Row; //oMatrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                            string ItemCode = ObtenerArticuloMatrixPedidoCliente(oForm, eventInfo.Row);
                            if(ItemCode.Trim().Length > 0)
                            {
                                Funciones.Create_ContextMenu("Ver Imagen 1 Artículo BEAS", "Ver Imagen 1 Artículo " + ItemCode.Trim() + " en BEAS", -1);
                                Funciones.Create_ContextMenu("Ver Imagen 2 Artículo BEAS", "Ver Imagen 2 Artículo " + ItemCode.Trim() + " en BEAS", -1);
                                Funciones.Create_ContextMenu("Ver Imagen 3 Artículo BEAS", "Ver Imagen 3 Artículo " + ItemCode.Trim() + " en BEAS", -1);
                                Funciones.AddUserDataSource(oForm, "UD_ROWMTX", SAPbouiCOM.BoDataType.dt_SHORT_NUMBER, 4);
                                oForm.DataSources.UserDataSources.Item("UD_ROWMTX").ValueEx = nRow.ToString();
                            }
                        }
                        else
                        {
                            Application.SBO_Application.Menus.RemoveEx("Ver Imagen 1 Artículo BEAS");
                            Application.SBO_Application.Menus.RemoveEx("Ver Imagen 2 Artículo BEAS");
                            Application.SBO_Application.Menus.RemoveEx("Ver Imagen 3 Artículo BEAS");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception) { }
        }

        private static string ObtenerArticuloMatrixPedidoCliente(SAPbouiCOM.Form oForm, int nRow)
        {
            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
            return ((SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(nRow).Specific).Value;
        }

        private static void  AbrirArchivoImagenArticuloBEAS(SAPbouiCOM.Form oForm,string sCampoImagen)
        {
            try 
	        {	        
		        int nRow = Convert.ToInt32(oForm.DataSources.UserDataSources.Item("UD_ROWMTX").ValueEx);
                SAPbobsCOM.Recordset rs = NConsultas.ConsultarArchivoImagenArticuloBEAS(ObtenerArticuloMatrixPedidoCliente(oForm, nRow));
                if (rs.RecordCount > 0)
                {
                    string sDire = rs.Fields.Item("BitmapPath").Value.ToString();
                    string sFile = rs.Fields.Item(sCampoImagen).Value.ToString();
                    if (sFile.Trim().Length>0) Funciones.Open_File(sDire.Trim() + sFile.Trim());
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
	        }
	        catch (Exception){}
        }


    }
}
