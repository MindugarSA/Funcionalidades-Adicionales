using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._2_Business_layer;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("1470000200", "3-Presentation Layer/System Forms/SolicitudCompra.b1f")]
    class SolicitudCompra : SystemFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbobsCOM.Company oCompany = Program.oCompany;
        private static SAPbouiCOM.ComboBox oComboBox = null;
        private static SAPbouiCOM.DBDataSource oDBDataSource = null;
        private static SAPbobsCOM.Items oItems = null;
        private static SAPbobsCOM.Recordset businessObject = null;

        public SolicitudCompra()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("10000329").Specific));
            this.ComboBox0.ClickBefore += new SAPbouiCOM._IComboBoxEvents_ClickBeforeEventHandler(this.ComboBox0_ClickBefore);
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
            //oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("13").Specific;
        }

        public static void SolicitudCompra_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bBubbleEvent)
        {
            bBubbleEvent = true;

            try
            {
                switch (pVal.BeforeAction)
                {
                    case true:
                        break;
                    case false:
                        if ((pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT) && (pVal.ItemUID == "10000329"))
                        {
                            if (oCompany == null)
                                Funciones.Connect_Application(ref oCompany);
                            oComboBox = (SAPbouiCOM.ComboBox)oForm.Items.Item(pVal.ItemUID).Specific;
                            businessObject = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            oItems = (SAPbobsCOM.Items)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                            int nOrden = 0;
                            int nDocOrd = 0;
                            string sSerie = "";
                            //string sql = "";
                            //string itemCode = "";
                            try
                            {
                                if (oComboBox.Selected.Description == "0")
                                {
                                    //Obtener DocEntry de Solicitud de Compra
                                    oDBDataSource = oForm.DataSources.DBDataSources.Item("OPRQ");
                                    nOrden = Convert.ToInt32(oDBDataSource.GetValue("DocNum", oDBDataSource.Offset)); //Convert.ToInt32(oEdit.Value);
                                    nDocOrd = Convert.ToInt32(oDBDataSource.GetValue("DocEntry", oDBDataSource.Offset));
                                    oComboBox = (SAPbouiCOM.ComboBox)oForm.Items.Item("88").Specific;
                                    sSerie = oComboBox.Selected.Description;


                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(businessObject);

                                    //Abrir Pantalla de Salida de Inventario y agregar lineas de acuerdo a Solicitud
                                    try
                                    {
                                        Application.SBO_Application.ActivateMenuItem("3079");
                                        oForm = Application.SBO_Application.Forms.ActiveForm;
                                        SalidaMercancias.Agregar_Items_Desde_Solicitud_Interna(nDocOrd, nOrden, sSerie);
                                    }
                                    catch (Exception) { }

                                }
                            }
                            catch (Exception) { }
                        }
                        break;
                }
            }
            catch (Exception) { }
        }

        private void ComboBox0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
            SAPbouiCOM.ComboBox oComboBox = (SAPbouiCOM.ComboBox)oForm.Items.Item("88").Specific;

            if (ComboBox0.Item.Enabled == true && (oComboBox.Selected.Description == "SCI" || oComboBox.Selected.Description == "SIP"))
            {
                try
                {
                    ComboBox0.ValidValues.Add("Salida de Mercancias", "0");
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    ComboBox0.ValidValues.Remove(2);
                }
                catch (Exception) { }
            }

        }

        private SAPbouiCOM.ComboBox ComboBox0;

    }
}
