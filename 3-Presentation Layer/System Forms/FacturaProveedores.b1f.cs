using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace FuncionalidadesAdicionales._3_Presentation_Layer.System_Forms
{
    [FormAttribute("141", "3-Presentation Layer/System Forms/FacturaProveedores.b1f")]
    class FacturaProveedores : SystemFormBase
    {

        private static SAPbouiCOM.Form oForm = null;
        private static SAPbobsCOM.Company oCompany = Program.oCompany;
        private static SAPbouiCOM.DataTable oDataTable = null;

        public FacturaProveedores()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));
            this.DT_SQL2 = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL2")));
            this.DT_SQL3 = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL3")));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataAddAfter += new DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataUpdateAfter += new DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);

        }

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);

            if (Program.AbiertoDesdeEnlace)
            {
                try
                {
                    Program.AbiertoDesdeEnlace = false;
                    Habilita_Importacion_Anexos();
                }
                catch (Exception)
                { }
            }
        }

        public static void FacturaProveedores_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BOInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //Muestra en las Ordenes de Compra (Pedidos Compras) los saldos correspondientes a los pagos relacionados realizados.
            if ((BOInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD) && !(BOInfo.BeforeAction))
            {
                try
                {
                    //oForm = Application.SBO_Application.Forms.Item(BOInfo.FormUID);
                    try
                    {
                        oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");
                    }
                    catch (Exception)
                    {
                        Program.AbiertoDesdeEnlace = true;
                    }
                }

                catch
                {
                    // Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
                }
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            Habilita_Importacion_Anexos();
        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            Habilita_Importacion_Anexos();
        }

        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            Habilita_Importacion_Anexos();
        }

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
            try
            {
                if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    Habilita_Importacion_Anexos();
            }
            catch (Exception)
            {
            }
        }

        //BOTON IMPORTAR ANEXOS DESDE DOCUMENTOS BASE
        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (Button0.Item.Enabled)
            {
                Importar_Anexos_Desde_Origen();
            }
        }

        private void Habilita_Importacion_Anexos()
        {
            //if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            //{
            try
            {
                //Verifica si existe al menos una linea con un Documento de Origen
                SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("PCH1");
                string sBaseType = "-1";

                for (int i = 0; i < source.Size; i++)
                {
                    sBaseType = (source.GetValue("BaseType", i));
                    if (sBaseType.Trim() != "-1")
                        break;
                }

                if (sBaseType.Trim() != "-1")
                    Button0.Item.Enabled = true;
                else
                    Button0.Item.Enabled = false;

            }
            catch (Exception) { }
            //}
        }


        private void Importar_Anexos_Desde_Origen()
        {
            try
            {
                oForm.Freeze(true);

                SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("OPCH");
                string sDocEntry = source.GetValue("DocEntry", source.Offset);
                int iAtcEntry = int.TryParse(source.GetValue("AtcEntry", source.Offset), out iAtcEntry) ? iAtcEntry : 0;
                //SAPbobsCOM.Documents oPurchaseOrders = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                //oPurchaseOrders.GetByKey(Convert.ToInt32(sDocEntry));

                string sql = "select  distinct BaseRef,BaseType,BaseEntry from PCH1 where docentry = " + sDocEntry;
                DT_SQL.ExecuteQuery(sql);

                if (!DT_SQL.IsEmpty)
                {
                    for (int i = 0; i < DT_SQL.Rows.Count; i++)
                    {
                        SAPbobsCOM.Recordset oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        SAPbouiCOM.Matrix oMatrix = null;
                        SAPbouiCOM.EditText oEdit = null;

                        string sBaseType = DT_SQL.GetValue("BaseType", i).ToString();
                        string sBaseRef = DT_SQL.GetValue("BaseRef", i).ToString();
                        string sBaseEntry = DT_SQL.GetValue("BaseEntry", i).ToString();
                        string sTable = "";

                        switch (sBaseType)
                        {
                            case "20": //Orden Compra
                                sql = "select  distinct BaseRef,BaseType,BaseEntry from PDN1 where docentry = " + sBaseEntry;
                                DT_SQL3.ExecuteQuery(sql);
                                if (!DT_SQL3.IsEmpty)
                                {
                                    sBaseType = DT_SQL3.GetValue("BaseType", 0).ToString();
                                    if (sBaseType == "22")
                                    {
                                        sTable = "OPOR";
                                        sBaseRef = DT_SQL3.GetValue("BaseRef", 0).ToString();
                                    }
                                }
                                break;
                            case "22": //Orden Compra
                                sTable = "OPOR";
                                break;
                            case "1470000113": //Solicitud de Compra
                                sTable = "OPRQ";
                                break;
                        }

                        if (sTable.Trim().Length > 0)
                        {
                            sql = "SELECT AtcEntry FROM " + sTable + " WHERE AtcEntry is not null AND DocNum = " + sBaseRef;
                            DT_SQL2.ExecuteQuery(sql);

                            if (!DT_SQL2.IsEmpty)
                            {
                                SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                                oAtt.GetByKey(Convert.ToInt32(DT_SQL2.GetValue("AtcEntry", 0)));

                                //SAPbobsCOM.Attachments2 oAttN = (SAPbobsCOM.Attachments2)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                                //if (iAtcEntry > 0)
                                //{
                                //    oAttN.GetByKey(iAtcEntry);
                                //}

                                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("1320002138").Specific;

                                int rowNum = oMatrix.RowCount + 1;

                                sql = @"select AttachPath from OADP";
                                oRecordset.DoQuery(sql);
                                string RutaDestino = oRecordset.Fields.Item(0).Value.ToString();

                                for (int x = 0; x < oAtt.Lines.Count; x++)
                                {

                                    sql = @"SELECT RTRIM(CAST(trgtPath  as nvarchar(200)))+'\'+FileName+'.'+FileExt from ATC1 where AbsEntry = " + DT_SQL2.GetValue("AtcEntry", 0) + " AND Line = " + (x + 1).ToString();
                                    oRecordset.DoQuery(sql);
                                    string RutaServ = oRecordset.Fields.Item(0).Value.ToString();

                                    oMatrix.AddRow();

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000002").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = RutaDestino;

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000003").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = System.IO.Path.GetDirectoryName(RutaServ);

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000004").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = System.IO.Path.GetFileNameWithoutExtension(RutaServ);

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000005").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = System.IO.Path.GetExtension(RutaServ).Substring(1);

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000007").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = DateTime.Now.ToShortDateString();

                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000001").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = rowNum.ToString();

                                    //Se vuelve a cargar la primera fila para que no avance el scroll horizontal al final.
                                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000002").Cells.Item(rowNum).Specific;
                                    oEdit.Active = true;
                                    oEdit.Item.Enabled = true;
                                    oEdit.String = RutaDestino;

                                    rowNum += 1;

                                    //oAtt.Lines.SetCurrentLine(x);
                                    //oAttN.Lines.Add();
                                    //oAttN.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(RutaServ);
                                    //oAttN.Lines.FileExtension = System.IO.Path.GetExtension(RutaServ).Substring(1);
                                    //oAttN.Lines.SourcePath = System.IO.Path.GetDirectoryName(RutaServ);
                                    //oAttN.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                                    //if (iAtcEntry > 0)
                                    //{
                                    //    if (oAttN.Update() != 0)
                                    //        throw new Exception(oCompany.GetLastErrorDescription());  
                                    //}
                                    //else
                                    //{
                                    //    int iAttEntry = -1;

                                    //    if (oAttN.Add() == 0)
                                    //    {
                                    //        iAttEntry = int.Parse(oCompany.GetNewObjectKey());
                                    //        //Assign the attachment to the GR object (GR is my SAPbobsCOM.Documents object)  
                                    //        oPurchaseOrders.AttachmentEntry = iAttEntry;
                                    //    }  
                                    //}

                                }

                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                            }
                        }


                    }
                }

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oPurchaseOrders);
                //oPurchaseOrders = null;
                GC.Collect();
            }
            catch (Exception)
            {
            }
            finally
            {
                oForm.Freeze(false);
            }
        }
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.DataTable DT_SQL;
        private SAPbouiCOM.DataTable DT_SQL2;
        private SAPbouiCOM.DataTable DT_SQL3;


    }
}
