using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._2_Business_layer;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("426", "3-Presentation Layer/System Forms/PagoEfectuado.b1f")]
    class PagoEfectuado : SystemFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbouiCOM.DBDataSource oDBDataSource = null;
        private static SAPbouiCOM.DataTable oDataTable = null;
        public static SAPbobsCOM.Company oCompany = Program.oCompany;

        public PagoEfectuado()
        {
        }
        ///-- MODIFICACIONES SOBRE EL FORM --
        /// Para poder distingir cuando un pago emitido contiene una transferencia bancaria, se creo una tabla [@ZAUTORI]
        /// la cual se maneja por la clase DTablaAutoriza y NTablaAutoriza donde al momento de guardar un pago, verifica 
        /// si este tiene un pago por transferencia, asi registra en dicha tabla el registro correspondiente para que sea
        /// leido por la Query para los Procesos de Autorizacion (Pago Efectuado con Transferencia)
        /// 
        /// 
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            try
            {
                this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));
            }
            catch (Exception) { }

            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
        }

        public static void PagoEfectuado_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bBubbleEvent)
        {
            bBubbleEvent = true;
            try
            {
                //    switch (pVal.BeforeAction)
                //    {
                //        case true:
                //            //Al momento de crear la Orden de Compra, compara las lineas del documento con el documento base para verificar que estas sean identicas
                //            if ((pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && (pVal.ItemUID == "1"))
                //            {
                //                try
                //                {
                //                    oButton = (SAPbouiCOM.Button)oForm.Items.Item(pVal.ItemUID).Specific;
                //                    if (!Funciones.DataTableExists(oForm, "DT_SQL"))
                //                        oForm.DataSources.DataTables.Add("DT_SQL");
                //                    oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");

                //                    if (oButton.Caption == "Crear")
                //                        OrdenCompra.Comparar_Lineas_Solicitud(pVal.FormUID);
                //                }
                //                catch (Exception) { }
                //            }
                //            break;
                //        case false:
                //            //Muestra en las Ordenes de Compra (Pedidos Compras) los saldos correspondientes a los pagos relacionados realizados.
                //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE)
                //            {
                //                if (Funciones.ItemExists(oForm, "Item_0") == true)
                //                {
                //                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific;
                //                    if (oEdit.Value.Trim().Length == 0)
                //                        Funciones.CargarDatosPagos(oForm);
                //                }
                //            }
                //            break;
                //    }
            }
            catch (Exception) { }
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (Button0.Item.Enabled)
            {
                try
                {
                    oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);

                    if (!Funciones.DataTableExists(oForm, "DT_SQL"))
                        oForm.DataSources.DataTables.Add("DT_SQL");
                    oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");
                    oDBDataSource = oForm.DataSources.DBDataSources.Item("OVPM");
                    string sDocNum = oDBDataSource.GetValue("DocNum", oDBDataSource.Offset).ToString();
                    string sDocEntry = oDBDataSource.GetValue("DocEntry", 0) == "" ? "0" : (oDBDataSource.GetValue("DocEntry", 0));
                    string sCardCode = oDBDataSource.GetValue("CardCode", oDBDataSource.Offset).ToString();
                    double dTrsfrSum = Funciones.GetDoubleFromString(oDBDataSource.GetValue("TrsfrSum", oDBDataSource.Offset).ToString());
                    double dDocTotal = Funciones.GetDoubleFromString(oDBDataSource.GetValue("DocTotal", oDBDataSource.Offset).ToString());

                    //PARA AUTORIZAR CHEQUES
                    double dCheckSum = Funciones.GetDoubleFromString(oDBDataSource.GetValue("CheckSum", oDBDataSource.Offset).ToString());
                    string sCheckAutor = oDBDataSource.GetValue("U_Cheque_Autor", oDBDataSource.Offset).ToString();


                    if (sCardCode.Trim().Length != 0 && dDocTotal > 0)
                    {
                        string sAprovved = "0";

                        if (dTrsfrSum == 0)
                            sAprovved = "1";
                        else
                            sAprovved = "0";

                        //PARA AUTORIZAR CHEQUES
                        if (dCheckSum > 0 && sCheckAutor.Trim() == "Si")
                            sAprovved = "0";

                        oDataTable = NTablaAutoriza.BuscarDatosAutorizacion(oDataTable, "46", sDocNum);
                        if (!oDataTable.IsEmpty)
                            for (int i = 0; i <= oDataTable.Rows.Count - 1; i++)
                            {
                                string sCodeUDO = oDataTable.GetValue("Code", i).ToString();
                                NTablaAutoriza.EliminarDatosAutorizacion(sCodeUDO);
                            }


                        NTablaAutoriza.InsertarDatosAutorizacion("46",
                                                                    sDocEntry,
                                                                    sDocNum,
                                                                    sAprovved,
                                                                    "",
                                                                    Convert.ToString(Program.oCompany.UserSignature),
                                                                    DateTime.Now.ToString("MMM dd yyyy h:ss tt"));

                    }
                }
                catch (Exception) { }
            }
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.DataTable DT_SQL;


    }
}
