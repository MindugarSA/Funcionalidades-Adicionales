using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms
{
    [FormAttribute("FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms.FrmEmpleados", "3-Presentation Layer/Users Forms/FrmEmpleados.b1f")]
    class FrmEmpleados : UserFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbouiCOM.UserDataSource oUDS = null;
        private static string Ultima_Busqueda = "";

        public FrmEmpleados()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid3 = ((SAPbouiCOM.Grid)(this.GetItem("Item_2").Specific));
            this.Grid3.ClickBefore += new SAPbouiCOM._IGridEvents_ClickBeforeEventHandler(this.Grid3_ClickBefore);
            this.Grid3.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid3_ClickAfter);
            this.Grid3.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid3_DoubleClickAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button1.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button1_ClickAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button2.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button2_ClickAfter);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_7").Specific));
            this.EditText0.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.EditText0_KeyDownAfter);
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_9").Specific));
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));

            try //Asignar Pantalla comno Modal
            {
                this.UIAPIRawForm.ReportType = "Modal";
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

            BuscarDatos();
        }

        private SAPbouiCOM.Grid Grid3;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.DataTable DT_SQL;


        private void Grid3_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                Grid3.Rows.SelectedRows.Add(pVal.Row);
            }
            catch (Exception)
            {
            }
        }

        private void Grid3_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void Grid3_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Button0.Item.Click();
        }

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (CheckBox0.Checked)
                {
                    oUDS = oForm.DataSources.UserDataSources.Item("UD_0");

                    SAPbouiCOM.Form oFormP = Application.SBO_Application.Forms.Item(oUDS.ValueEx);
                    oForm.Close();

                    ((SAPbouiCOM.EditText)oFormP.Items.Item("Item_2").Specific).String = "SIN RUT";
                    ((SAPbouiCOM.EditText)oFormP.Items.Item("Item_10").Specific).String = "SIN RUT ASOCIADO";
                }
                else
                if (Grid3.Rows.SelectedRows.Count > 0)
                {
                    oUDS = oForm.DataSources.UserDataSources.Item("UD_0");
                    string sRut = Convert.ToString(Grid3.DataTable.GetValue(0, Grid3.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder)));
                    string sNombre = Convert.ToString(Grid3.DataTable.GetValue(1, Grid3.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder)));

                    SAPbouiCOM.Form oFormP = Application.SBO_Application.Forms.Item(oUDS.ValueEx);
                    oForm.Close();

                    ((SAPbouiCOM.EditText)oFormP.Items.Item("Item_2").Specific).String = sRut;
                    ((SAPbouiCOM.EditText)oFormP.Items.Item("Item_10").Specific).String = sNombre;


                }

            }
            catch (Exception)
            {
            }
        }

        private void Button1_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            oForm.Close();
        }

        private void Button2_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            BuscarDatos();
        }

        private void EditText0_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                switch (pVal.CharPressed)
                {
                    case 13:
                        if (EditText0.Value.Trim() == Ultima_Busqueda || (EditText0.Value.Trim().Length == 0 && Ultima_Busqueda == "°#TODOS LOS REGISTROS#°") && Grid3.Rows.SelectedRows.Count > 0)
                        { Button0.Item.Click(); }
                        else { Button1.Item.Click(); }
                        break;
                    case 40:
                        if (Grid3.Rows.Count > 0)
                        {
                            int nRow = Grid3.Rows.SelectedRows.Count == 0 ? -1 : Grid3.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                            Grid3.Rows.SelectedRows.Add(nRow < 0 ? 0 : nRow + 1);
                        }
                        break;
                    case 38:
                        if (Grid3.Rows.Count > 0)
                        {
                            int nRow = Grid3.Rows.SelectedRows.Count == 0 ? -1 : Grid3.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                            Grid3.Rows.SelectedRows.Add(nRow < 0 ? 0 : nRow - 1);
                        }
                        break;

                }
            }
            catch (Exception)
            {
            }
        }

        private void BuscarDatos()
        {
            try
            {
                string sql = "";
                if (EditText0.Value.Trim().Length > 0)
                {
                    sql = @" exec [SBO_COMERCIAL].[dbo].[SpEmpleadosTodosBuscar] '" + EditText0.Value.Trim() + "'";

                    DT_SQL.ExecuteQuery(sql);

                    Ultima_Busqueda = EditText0.Value.Trim();

                }
                else
                {
                    sql = @" exec [SBO_COMERCIAL].[dbo].[SpEmpleadosTodosBuscar] ''";

                    DT_SQL.ExecuteQuery(sql);

                    Ultima_Busqueda = "°#TODOS LOS REGISTROS#°";
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
