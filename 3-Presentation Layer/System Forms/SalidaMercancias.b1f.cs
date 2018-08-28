using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using SAPbobsCOM;
using FuncionalidadesAdicionales._2_Business_layer;
using System.Xml;
using FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("720", "3-Presentation Layer/System Forms/SalidaMercancias.b1f")]
    class SalidaMercancias : SystemFormBase
    {

        private static SAPbobsCOM.Company oCompany = Program.oCompany ;
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbouiCOM.EditText oEdit = null;

        public SalidaMercancias()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_0").Specific));
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("13").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_9").Specific));
            this.Button1.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button1_ClickAfter);
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("Item_10").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);

        }

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
            //EditText1.Item.AffectsFormMode = false;
            //EditText1.DataBind.SetBound(true, "OIGE", "U_Solicitud_Compra");
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_2").Specific;

            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                if (oEdit.Value.Trim().Length == 0)
                {
                    Application.SBO_Application.StatusBar.SetText("Debe Seleccionar el Empleado que Recibe", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    BubbleEvent = false;
                }
                else
                {

                    if (EditText1.Value.Trim().Length > 0)
                    {

                        if (!DT_SQL.IsEmpty)
                        {

                            string nOrden = "";
                            string nLinea = "";
                            decimal nCant = 0;

                            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("13").Specific;

                            for (int i = 0; i < oMatrix.RowCount - 1; i++)
                            {
                                nOrden = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("231000074").Cells.Item(i).Specific).Value;
                                nLinea = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("24").Cells.Item(i).Specific).Value;
                                nCant = Convert.ToDecimal(((SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(i).Specific).Value);


                            }
                        }
                    }
                }


        }

        private void Button1_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                FrmEmpleados activeForm = new FrmEmpleados();

                SAPbouiCOM.UserDataSource oUDS = activeForm.UIAPIRawForm.DataSources.UserDataSources.Item("UD_0");
                oUDS.ValueEx = oForm.UniqueID;

                activeForm.Show();
            }
            catch { }

        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {

            SAPbouiCOM.DBDataSource oDBDataSource = oForm.DataSources.DBDataSources.Item("OIGE");
            SAPbouiCOM.DBDataSource oDBDataSourceLines = oForm.DataSources.DBDataSources.Item("IGE1");
            oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("7").Specific;
            int sDocNum = Convert.ToInt32(oDBDataSource.GetValue("DocNum", oDBDataSource.Offset));
            int sDocEntry = Convert.ToInt32(oDBDataSource.GetValue("DocEntry", oDBDataSource.Offset));
            string sql = "";

            if (((SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific).Value.Trim() != "")
            {
                try
                {
                    int nDocOrd = Convert.ToInt32(oDBDataSource.GetValue("U_Solicitud_Compra", oDBDataSource.Offset).ToString().Substring(4, 9));  //;Convert.ToInt32(DT_SQL.GetValue("U_Solicitud_Compra", 0).ToString().Substring(4,9));

                    sql = @"UPDATE D SET BaseLine = ImportLog , BaseEntry = S.DocEntry
		                from OIGE S JOIN IGE1 D ON S.DocEntry = D.DocEntry
		                where docnum = " + sDocNum;

                    DT_SQL.ExecuteQuery(sql);

                    sql = @"UPDATE D SET OpenQty = IIF(SAL.Entregado IS NULL, D.Quantity ,(D.Quantity - SAL.Entregado)),
                        OpenCreQty = IIF(SAL.Entregado IS NULL, D.Quantity ,(D.Quantity - SAL.Entregado)),
                        LineStatus = IIF((D.Quantity - SAL.Entregado) = 0,'C','O')
		                FROM OPRQ P JOIN PRQ1 D ON P.DocEntry = D.DocEntry
		                LEFT JOIN (select BaseRef,BaseLine,sum(D.Quantity) as Entregado
				                from OIGE S JOIN IGE1 D ON S.DocEntry = D.DocEntry
				                where BaseRef is not null
				                GROUP BY BaseRef,BaseLine) SAL ON P.DocNum = SAL.BaseRef AND D.LineNum = SAL.BaseLine
		                WHERE P.DocNum = " + nDocOrd;

                    DT_SQL.ExecuteQuery(sql);

                    sql = @"UPDATE T0 SET DocStatus = IIF(Cerradas = Lineas,'C','O')
		                    FROM OPRQ T0
		                    JOIN (select P.DocEntry,SUM(IIF(LineStatus = 'C',1,0)) as Cerradas,COUNT(D.LineNum) as Lineas
		                    FROM OPRQ P JOIN PRQ1 D ON P.DocEntry = D.DocEntry
                            WHERE P.DocNum = " + nDocOrd + @"
		                    GROUP BY P.DocEntry) T1 ON T0.DocEntry = T1.DocEntry
		                    WHERE  docnum = " + nDocOrd;

                    DT_SQL.ExecuteQuery(sql);

                    //SAPbobsCOM.Documents oInventoryGenExit;
                    //SAPbobsCOM.Document_Lines oInventoryGenExitLines;

                    //Get InventoryGenExit Object
                    //oInventoryGenExit = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                    //if (oInventoryGenExit.GetByKey(sDocEntry))
                    //{
                    //    oInventoryGenExitLines = oInventoryGenExit.Lines;
                    //    for (int i = 0; i < oInventoryGenExitLines.Count; i++)
                    //    {
                    //        oInventoryGenExitLines.SetCurrentLine(i);
                    //        oInventoryGenExitLines.BaseLine = Convert.ToInt32(oDBDataSourceLines.GetValue("ImportLog", i));
                    //        oInventoryGenExitLines.BaseEntry = Convert.ToInt32(oDBDataSourceLines.GetValue("DocEntry", i));
                    //    }

                    //    if (oInventoryGenExit.Update() != 0)
                    //    {
                    //        int a = oCompany.GetLastErrorCode(); 
                    //        string b = oCompany.GetLastErrorDescription();
                    //    }
                    //}

                    //Documents oPurchaseRequest = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                    //int nCantArtClose = 0;

                    //if (oPurchaseRequest.GetByKey(nDocOrd))
                    //{
                    //    for (int i = 0; i < oPurchaseRequest.Lines.Count; i++)
                    //    {
                    //        oPurchaseRequest.Lines.SetCurrentLine(i);
                    //        if (oPurchaseRequest.Lines.RemainingOpenQuantity == 0)
                    //        {
                    //            oPurchaseRequest.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close;
                    //            nCantArtClose += 1;
                    //        }
                    //    }

                    //    if (nCantArtClose == oPurchaseRequest.Lines.Count)
                    //        oPurchaseRequest.Close();

                    //    int a = oPurchaseRequest.Update();
                    //}

                }
                catch (Exception)
                {
                }

            }

        }

        public static void Agregar_Items_Desde_Solicitud_Interna(int nDocOrd, int nOrden, string sSerie)
        {
            Recordset businessObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Items oItems = (SAPbobsCOM.Items)oCompany.GetBusinessObject(BoObjectTypes.oItems);

            SAPbouiCOM.Matrix oMatrix = null;
            SAPbouiCOM.ComboBox oComboBox = null;

            string itemCode = "";

            try
            {
                oForm.Freeze(true);

                SAPbouiCOM.DBDataSource source = oForm.DataSources.DBDataSources.Item("IGE1");

                Documents oPurchaseRequest = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                double nCantArt = 0;
                int iSal = 0;

                if (oPurchaseRequest.GetByKey(nDocOrd))
                {
                    oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("13").Specific;
                    for (int i = 0; i < oPurchaseRequest.Lines.Count; i++)
                    {
                        oPurchaseRequest.Lines.SetCurrentLine(i);
                        itemCode = oPurchaseRequest.Lines.ItemCode;
                        if (oItems.GetByKey(itemCode) && oPurchaseRequest.Lines.RemainingOpenQuantity > 0)
                        {

                            oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(iSal + 1).Specific;
                            oEdit.String = oPurchaseRequest.Lines.ItemCode;

                            oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("9").Cells.Item(iSal + 1).Specific;
                            oEdit.String = Convert.ToString(oPurchaseRequest.Lines.RemainingOpenQuantity);

                            oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("231000074").Cells.Item(iSal + 1).Specific;
                            oEdit.Active = true;
                            oEdit.Item.Enabled = true;
                            oEdit.String = Convert.ToString(nOrden);

                            oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("24").Cells.Item(iSal + 1).Specific;
                            oEdit.Active = true;
                            oEdit.Item.Enabled = true;
                            oEdit.String = Convert.ToString(oPurchaseRequest.Lines.LineNum);

                            nCantArt += oPurchaseRequest.Lines.Quantity;
                            iSal += 1;
                        }
                    }


                    //Agregar como Referencia Numero de SCI
                    oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("Item_0").Specific;
                    oEdit.String = sSerie.Trim() + "-" + Convert.ToString(nOrden);
                    oComboBox = (SAPbouiCOM.ComboBox)oForm.Items.Item("30").Specific;
                    oComboBox.Select("SI", SAPbouiCOM.BoSearchKey.psk_ByDescription);

                    SAPbouiCOM.UserDataSource oUds = oForm.DataSources.UserDataSources.Item("UD_SCI");
                    oUds.ValueEx = nDocOrd.ToString();
                    oUds = oForm.DataSources.UserDataSources.Item("UD_CANT");
                    oUds.ValueEx = nCantArt.ToString();

                    oPurchaseRequest.Lines.SetCurrentLine(iSal - 1);
                    itemCode = oPurchaseRequest.Lines.ItemCode;
                    oEdit = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_FAS_Kilos_Unitario").Cells.Item(iSal).Specific;
                    oEdit.Active = true;
                    oEdit.Item.Enabled = true;
                    oEdit.Item.Click();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oPurchaseRequest);
                    oPurchaseRequest = null;
                    GC.Collect();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                oForm.Freeze(false);
                Application.SBO_Application.SendKeys("{TAB}");
            }
        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.DataTable DT_SQL;        
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText EditText3;
        private SAPbouiCOM.StaticText StaticText3;


    }
}
