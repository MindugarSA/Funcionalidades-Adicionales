using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("FuncionalidadesAdicionales.frmBuscaEmp", "3-Presentation Layer/Users Forms/frmBuscaEmp.b1f")]
    class frmBuscaEmp : UserFormBase
    {

        private static SAPbouiCOM.Form oForm = null;
        private static SAPbouiCOM.UserDataSource oUDS = null;
        private static string Ultima_Busqueda = "";

        public frmBuscaEmp()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.EditText1.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.EditText1_KeyDownAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button1.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button1_ClickAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("Item_7").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
           
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_10").Specific));
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
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_9").Specific));
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.Grid0.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid0_ClickAfter);

            Grid0.Item.FromPane = 1;
            Grid0.Item.ToPane = 1;

            Grid0.Columns.Item("SerialNo").Click(0, false, 0);

            BuscarDatos();
        }

        private void Grid0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                Grid0.Rows.SelectedRows.Add(pVal.Row);
            }
            catch (Exception)
            {
            }
        }

        private void Grid0_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Button1.Item.Click();
        }

        private void Button1_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
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
                if (Grid0.Rows.SelectedRows.Count > 0)
                {
                    oUDS = oForm.DataSources.UserDataSources.Item("UD_0");
                    string sRut = Convert.ToString(Grid0.DataTable.GetValue(0, Grid0.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder)));
                    string sNombre = Convert.ToString(Grid0.DataTable.GetValue(1, Grid0.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder)));

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

        private void EditText1_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                switch (pVal.CharPressed)
                {
                    case 13:
                        if (EditText1.Value.Trim() == Ultima_Busqueda || (EditText1.Value.Trim().Length == 0 && Ultima_Busqueda == "°#TODOS LOS REGISTROS#°") && Grid0.Rows.SelectedRows.Count > 0)
                        { Button0.Item.Click(); }
                        else { Button1.Item.Click(); }
                        break;
                    case 40:
                        if (Grid0.Rows.Count > 0)
                        {
                            int nRow = Grid0.Rows.SelectedRows.Count == 0 ? -1 : Grid0.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                            Grid0.Rows.SelectedRows.Add(nRow < 0 ? 0 : nRow + 1);
                        }
                        break;
                    case 38:
                        if (Grid0.Rows.Count > 0)
                        {
                            int nRow = Grid0.Rows.SelectedRows.Count == 0 ? -1 : Grid0.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                            Grid0.Rows.SelectedRows.Add(nRow < 0 ? 0 : nRow - 1);
                        }
                        break;

                }
            }
            catch (Exception)
            {
            }
        }

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            BuscarDatos();
        }

        private void BuscarDatos()
        {
            try
            {
                string sql = "";
                if (EditText1.Value.Trim().Length > 0)
                {
                    sql = @" exec [SBO_COMERCIAL].[dbo].[SpEmpleadosTodosBuscar] '" + EditText1.Value.Trim() + "'";

                    DT_SQL.ExecuteQuery(sql);

                    Ultima_Busqueda = EditText1.Value.Trim();

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

        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Folder Folder0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.DataTable DT_SQL;
        private SAPbouiCOM.Grid Grid0;



    }
}
