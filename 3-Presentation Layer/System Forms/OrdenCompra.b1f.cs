using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._2_Business_layer;
using FuncionalidadesAdicionales._1_Data_Layer;
using FuncionalidadesAdicionales._0___DTO;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("142", "3-Presentation Layer/System Forms/OrdenCompra.b1f")]
    class OrdenCompra : SystemFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbobsCOM.Company oCompany = Program.oCompany;
        private static SAPbouiCOM.DataTable oDataTable = null;
        private static SAPbouiCOM.UserDataSource oUserDataSource = null ;
        private static SAPbouiCOM.Matrix oMatrix = null;
        private static SAPbouiCOM.EditText oEdit = null;
        private static SAPbouiCOM.Button oButton = null;

        public OrdenCompra()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_0").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));
            this.DT_SQL2 = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL2")));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataUpdateAfter += new SAPbouiCOM.Framework.FormBase.DataUpdateAfterHandler(this.Form_DataUpdateAfter);
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);

        }

        private SAPbouiCOM.EditText EditText0;

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);

            if (Program.AbiertoDesdeEnlace)
            {
                try
                {
                    Program.AbiertoDesdeEnlace = false;
                    Funciones.CargarDatosPagos(oForm);
                    Habilita_Importacion_Anexos();

                }
                catch (Exception)
                {                }
            }
        }

        public static void OrdenCompra_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            // Boton CREAR Y BUSCAR
            if ((pVal.MenuUID == "1281" || pVal.MenuUID == "1282") && pVal.BeforeAction == false)
            {
                try
                {
                    oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_TP");
                    oUserDataSource.ValueEx = "";
                    oUserDataSource = oForm.DataSources.UserDataSources.Item("UD_SP");
                    oUserDataSource.ValueEx = "";
                }
                catch{}
            }
        }

        public static void OrdenCompra_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool bBubbleEvent)
        {
            bBubbleEvent = true;
            try
            {
                switch (pVal.BeforeAction)
                {
                    case true:
                        //Al momento de crear la Orden de Compra, compara las lineas del documento con el documento base para verificar que estas sean identicas
                        if ((pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) && (pVal.ItemUID == "1"))
                        {
                            try
                            {
                                oButton = (SAPbouiCOM.Button)oForm.Items.Item(pVal.ItemUID).Specific;
                                if (!Funciones.DataTableExists(oForm, "DT_SQL"))
                                    oForm.DataSources.DataTables.Add("DT_SQL");
                                oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");

                                if (oButton.Caption == "Crear")
                                    OrdenCompra.Comparar_Lineas_Solicitud(pVal.FormUID);
                            }
                            catch (Exception) { }
                        }
                        break;
                    case false:
                        //Muestra en las Ordenes de Compra (Pedidos Compras) los saldos correspondientes a los pagos relacionados realizados.
                        if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE)
                        {
                            if (Funciones.ItemExists(oForm, "Item_0") == true)
                            {
                                oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific;
                                if (oEdit.Value.Trim().Length == 0)
                                    Funciones.CargarDatosPagos(oForm);
                            }
                        }
                        break;
                }
            }
            catch (Exception) { }
        }

        public static void OrdenCompra_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BOInfo, out bool BubbleEvent)
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
                        Funciones.CargarDatosPagos(oForm);
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

        private void Importar_Anexos_Desde_Origen()
        {
            try
            {
                oForm.Freeze(true);

                SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("OPOR");
                string sDocEntry = source.GetValue("DocEntry", source.Offset);
                int iAtcEntry = int.TryParse(source.GetValue("AtcEntry", source.Offset), out iAtcEntry) ? iAtcEntry : 0;
                //SAPbobsCOM.Documents oPurchaseOrders = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                //oPurchaseOrders.GetByKey(Convert.ToInt32(sDocEntry));

                //source = oForm.DataSources.DBDataSources.Item("POR1");
                //for (int i = 0; i <= source.Size - 1; i++)
                //{
                //    var BaseRef = source.GetValue("BaseRef", i);
                //    var BaseTypex = source.GetValue("BaseType", i);
                //    var BaseEntryx = source.GetValue("BaseEntry", i);
                //}

                string BaseType = "";
                string TipoLinea = "";
                string sql = "";
                List<Anexos> lAnexos = new List<Anexos>();
                //List<Object> DetallesVoid = new List<Object>();
                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;

                // Recorre Matrix, cargar Objeto Anexo e insertar en lista
                for (int i = 1; i <= oMatrix.RowCount ; i++)
                {
                    try
                    {
                        BaseType = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("43").Cells.Item(i).Specific).Value.Trim();
                        TipoLinea = ((SAPbouiCOM.ComboBox)oMatrix.Columns.Item("257").Cells.Item(i).Specific).Selected.Value.Trim();
                    }
                    catch
                    {
                        TipoLinea = "No valido";
                        BaseType = "-1";
                    }

                    if (TipoLinea.Trim().Length == 0 && BaseType != "-1")
                    {

                        Anexos oAnexo = new Anexos();
                        oAnexo.FormID = oForm.UniqueID;
                        oAnexo.ObjType = oForm.TypeEx;
                        oAnexo.BaseRef = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("44").Cells.Item(i).Specific).Value.Trim();
                        oAnexo.BaseType = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("43").Cells.Item(i).Specific).Value.Trim();
                        oAnexo.BaseEntry = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("45").Cells.Item(i).Specific).Value.Trim();
                        lAnexos.Add(oAnexo);
                        //result = FuncionesUDO.InsertRecord("ZANEXOS", oAnexo, "", DetallesVoid);
                    }
                }

                //Distict a lista
               var ListAnexos = lAnexos
                                    .GroupBy(p => new { p.BaseRef,  p.BaseType, p.BaseEntry })
                                    .Select(g => g.First())
                                    .ToList();
                //recorrer lista para insertar en matrix de anexos
                if(ListAnexos.Count > 0)
                {
                    foreach (Anexos oAnexo in ListAnexos)
                    {
                        SAPbobsCOM.Recordset oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        SAPbouiCOM.Matrix oMatrix = null;
                        SAPbouiCOM.EditText oEdit = null;

                        string sBaseType = oAnexo.BaseType;
                        string sBaseRef = oAnexo.BaseRef;
                        string sBaseEntry = oAnexo.BaseEntry;
                        string sTable = "";

                        switch (sBaseType)
                        {
                            case "17": //Pedido
                                sTable = "ORDR";
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

                                }

                                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                            }
                        }
                    }
                }

                //string sql = "select  distinct BaseRef,BaseType,BaseEntry from POR1 where docentry = " + sDocEntry;
                //DT_SQL.ExecuteQuery(sql);

                //if (!DT_SQL.IsEmpty)
                //{
                //    for (int i = 0; i < DT_SQL.Rows.Count; i++)
                //    {
                //        SAPbobsCOM.Recordset oRecordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //        SAPbouiCOM.Matrix oMatrix = null;
                //        SAPbouiCOM.EditText oEdit = null;

                //        string sBaseType = DT_SQL.GetValue("BaseType", i).ToString();
                //        string sBaseRef = DT_SQL.GetValue("BaseRef", i).ToString();
                //        string sBaseEntry = DT_SQL.GetValue("BaseEntry", i).ToString();
                //        string sTable = "";

                //        switch (sBaseType)
                //        {
                //            case "17": //Pedido
                //                sTable = "ORDR";
                //                break;
                //            case "1470000113": //Solicitud de Compra
                //                sTable = "OPRQ";
                //                break;
                //        }

                //        if (sTable.Trim().Length > 0)
                //        {
                //            sql = "SELECT AtcEntry FROM " + sTable + " WHERE AtcEntry is not null AND DocNum = " + sBaseRef;
                //            DT_SQL2.ExecuteQuery(sql);

                //            if (!DT_SQL2.IsEmpty)
                //            {
                //                SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                //                oAtt.GetByKey(Convert.ToInt32(DT_SQL2.GetValue("AtcEntry", 0)));

                //                //SAPbobsCOM.Attachments2 oAttN = (SAPbobsCOM.Attachments2)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                //                //if (iAtcEntry > 0)
                //                //{
                //                //    oAttN.GetByKey(iAtcEntry);
                //                //}

                //                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("1320002138").Specific;

                //                int rowNum = oMatrix.RowCount + 1;

                //                sql = @"select AttachPath from OADP";
                //                oRecordset.DoQuery(sql);
                //                string RutaDestino = oRecordset.Fields.Item(0).Value.ToString();

                //                for (int x = 0; x < oAtt.Lines.Count; x++)
                //                {

                //                    sql = @"SELECT RTRIM(CAST(trgtPath  as nvarchar(200)))+'\'+FileName+'.'+FileExt from ATC1 where AbsEntry = " + DT_SQL2.GetValue("AtcEntry", 0) + " AND Line = " + (x + 1).ToString();
                //                    oRecordset.DoQuery(sql);
                //                    string RutaServ = oRecordset.Fields.Item(0).Value.ToString();

                //                    oMatrix.AddRow();

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000002").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = RutaDestino;

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000003").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = System.IO.Path.GetDirectoryName(RutaServ);

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000004").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = System.IO.Path.GetFileNameWithoutExtension(RutaServ);

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000005").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = System.IO.Path.GetExtension(RutaServ).Substring(1);

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000007").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = DateTime.Now.ToShortDateString();

                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000001").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = rowNum.ToString();

                //                    //Se vuelve a cargar la primera fila para que no avance el scroll horizontal al final.
                //                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1320000002").Cells.Item(rowNum).Specific;
                //                    oEdit.Active = true;
                //                    oEdit.Item.Enabled = true;
                //                    oEdit.String = RutaDestino;

                //                    rowNum += 1;

                //                    //oAtt.Lines.SetCurrentLine(x);
                //                    //oAttN.Lines.Add();
                //                    //oAttN.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(RutaServ);
                //                    //oAttN.Lines.FileExtension = System.IO.Path.GetExtension(RutaServ).Substring(1);
                //                    //oAttN.Lines.SourcePath = System.IO.Path.GetDirectoryName(RutaServ);
                //                    //oAttN.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                //                    //if (iAtcEntry > 0)
                //                    //{
                //                    //    if (oAttN.Update() != 0)
                //                    //        throw new Exception(oCompany.GetLastErrorDescription());  
                //                    //}
                //                    //else
                //                    //{
                //                    //    int iAttEntry = -1;

                //                    //    if (oAttN.Add() == 0)
                //                    //    {
                //                    //        iAttEntry = int.Parse(oCompany.GetNewObjectKey());
                //                    //        //Assign the attachment to the GR object (GR is my SAPbobsCOM.Documents object)  
                //                    //        oPurchaseOrders.AttachmentEntry = iAttEntry;
                //                    //    }  
                //                    //}

                //                }

                //                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                //            }
                //        }


                //    }
                //}

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

        private void Habilita_Importacion_Anexos()
        {
            //if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            //{
                try
                {
                    //Verifica si existe al menos una linea con un Documento de Origen
                    SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("POR1");
                    string sBaseType = "-1";

                    for (int i = 0; i < source.Size; i++)
                    {
                        sBaseType = (source.GetValue("BaseType", i));
                        if (sBaseType.Trim() != "-1")
                            break;
                    }

                    if (sBaseType.Trim() != "-1" || oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                        Button0.Item.Enabled = true;
                    else
                        Button0.Item.Enabled = false;

                }
                catch (Exception){}
            //}
        }

        public static void Comparar_Lineas_Solicitud(string FormID)
        {
            try
            {
                oForm = oForm = Application.SBO_Application.Forms.Item(FormID);
                oDataTable = oForm.DataSources.DataTables.Item("DT_SQL");
                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
                string Codigo = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(1).Specific).Value;
                SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("POR1");

                int nNumLineas = 0;
                int nDiferente = 0;
                string iBaseEntry = "0";
                string iBaseEntryAnt = "0";
                string iBaseLine = "0";
                string sBaseType = "-1";
                string sItemCode = "";
                string sDscription = "";
                string dQuantity = "";
                string dPrice = "";
                string dLineTotal = "";
                string sql = "";
                string sAutorizador = "";
                string sDocEntry = "";
                string sDocNum = "";
                string sDocTotal = "";
                string sVatSum = "";
                string sDiscSum = "";
                string sBrutoSolicitud = "";
                string sUserSign = "";


                for (int i = 1; i < oMatrix.RowCount; i++) //for (int i = 0; i < source.Size; i++) 
                {
                    sBaseType = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("43").Cells.Item(i).Specific).Value;//(source.GetValue("BaseType", i));
                    iBaseEntry = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("45").Cells.Item(i).Specific).Value; //(source.GetValue("BaseEntry", i));
                    iBaseLine = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("46").Cells.Item(i).Specific).Value; //(source.GetValue("BaseLine", i));
                    sItemCode = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(i).Specific).Value; //(source.GetValue("ItemCode", i));
                    sDscription = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("3").Cells.Item(i).Specific).Value; //(source.GetValue("Dscription", i));
                    dQuantity = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("11").Cells.Item(i).Specific).Value; //(source.GetValue("Quantity", i));
                    dPrice = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("14").Cells.Item(i).Specific).Value.Replace(".","|").Replace(",",".").Replace("|","").Replace("$",""); //(source.GetValue("Price", i));
                    dLineTotal = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("21").Cells.Item(i).Specific).Value.Replace(".", "|").Replace(",", ".").Replace("|", "").Replace("$", ""); //(source.GetValue("LineTotal", i));

                    if (sBaseType.Trim() != "1470000113") // Verifica que las lineas provengan de una solicitud de compra
                    {
                        nDiferente = 1;
                        break;
                    }

                    if (i > 1)
                    {
                        if (iBaseEntryAnt != iBaseEntry) //Verifica que los numeros de Solicitud sean iguales para cada linea
                        {
                            nDiferente = 1;
                            break;
                        }
                    }
                    else
                    {
                        iBaseEntryAnt = iBaseEntry; // Solicitud Base de la primera linea
                    }

                    sql = @"SELECT 
                                TM.UserID,(T0.DocTotal-T0.VatSum+T0.DiscSum) as 'Bruto Solicitud'
		                    FROM 
		                        OPRQ T0 JOIN OWDD T1 ON T0.DocEntry = T1.DocEntry AND T0.ObjType = T1.ObjType
		                        LEFT JOIN (SELECT AP.WddCode,AP.UserID
			                          FROM WDD1 AP JOIN (select wddcode,MAX(StepCode) as Ultimo FROM WDD1 GROUP BY WddCode) MX ON AP.WddCode = MX.WddCode AND AP.StepCode = MX.Ultimo) TM ON T1.WddCode = TM.WddCode  
		                        JOIN PRQ1 T2 ON T0.DocEntry = T2.DocEntry 
		                    WHERE 
                                T1.Status = 'Y' AND T2.DocEntry = " + iBaseEntry.ToString().Trim() + " AND T2.LineNum = " + iBaseLine.ToString().Trim() + @"
                                AND T2.[ItemCode] = '"+sItemCode+@"'
					            AND RTRIM(LTRIM(T2.[Dscription])) = '" + sDscription + @"'
					            AND T2.[Quantity] = " + dQuantity.ToString().Trim() + @"
					            AND T2.[Price] = " + dPrice.ToString().Trim() + @"
					            AND T2.[LineTotal] = " + dLineTotal.ToString().Trim() ;

                    oDataTable.ExecuteQuery(sql);
                    if (!oDataTable.IsEmpty)
                    {
                        nNumLineas += 1;
                        sAutorizador = oDataTable.GetValue("UserID", 0).ToString();
                        sBrutoSolicitud = oDataTable.GetValue("Bruto Solicitud", 0).ToString();
                    }
                    else
                    {
                        nDiferente = 1;
                        break;
                    }

                }

                if (nNumLineas != oMatrix.RowCount - 1)//(nNumLineas != source.Size)
                    nDiferente = 1;

                source = oForm.DataSources.DBDataSources.Item("OPOR");
                sDocEntry = (source.GetValue("DocEntry", 0)) == "" ? "0" : (source.GetValue("DocEntry", 0)) ;
                sDocNum = (source.GetValue("DocNum", 0));
                sDocTotal = (source.GetValue("DocTotal", 0));
                sVatSum = (source.GetValue("VatSum", 0));
                sDiscSum = (source.GetValue("DiscSum", 0));
                sUserSign = (source.GetValue("UserSign", 0)) == "" ? Convert.ToString(Program.oCompany.UserSignature) : (source.GetValue("UserSign", 0)) ;

                //sql = "DELETE FROM [@ZAUTORI] WHERE U_ObjType = 22 AND U_DocNum = ISNULL(" + sDocNum + ",0)";
                //oDataTable.ExecuteQuery(sql);
                oDataTable = NTablaAutoriza.BuscarDatosAutorizacion(oDataTable, "22" ,sDocNum);                
                if (!oDataTable.IsEmpty)
                    for (int i = 0; i <= oDataTable.Rows.Count - 1; i++)
                    {
                        string sCodeUDO = oDataTable.GetValue("Code", i).ToString();
                        NTablaAutoriza.EliminarDatosAutorizacion(sCodeUDO);
                    }

                NTablaAutoriza.InsertarDatosAutorizacion("22"
                                                          ,sDocEntry 
                                                          ,sDocNum 
			                                              ,nDiferente.ToString() 
			                                              ,sAutorizador
			                                              ,sUserSign 
                                                          ,DateTime.Now.ToString("MMM dd yyyy h:ss tt"));
//                sql = @"	INSERT INTO [dbo].[@ZAUTORI]
//			                       ([Code]
//			                       ,[Name]
//			                       ,[U_ObjType]
//			                       ,[U_DocEntry]
//                                   ,[U_DocNum]
//			                       ,[U_Approved]
//			                       ,[U_UserPrevDoc]
//			                       ,[U_UserSing]
//			                       ,[U_CreateDate])
//		                     VALUES
//			                       ((select isnull(max(CAST([Code] as int)),0)+1 as Proximo from [dbo].[@ZAUTORI]) 
//			                       ,(select isnull(max(CAST([Name] as int)),0)+1 as Proximo from [dbo].[@ZAUTORI]) 
//			                       ,'22'
//			                       ,'" + sDocEntry + @"'
//                                   ,'" + sDocNum + @"'
//			                       ,'" + nDiferente + @"'
//			                       ,'" + sAutorizador + @"'
//			                       ,'" + sUserSign + @"'
//			                       ,CONVERT(Datetime,GETDATE()))";
//                oDataTable.ExecuteQuery(sql);


            }
            catch (Exception){}
            
        }

        
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.DataTable DT_SQL;
        private SAPbouiCOM.DataTable DT_SQL2;
        private SAPbouiCOM.Button Button1;

    }
}
