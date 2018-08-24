using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms
{
    [FormAttribute("FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms.frmParametrosAdic", "3-Presentation Layer/Users Forms/frmParametrosAdic.b1f")]
    class frmParametrosAdic : UserFormBase
    {
        public frmParametrosAdic()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_3").Specific));
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("Item_1").Specific));
            this.DT_SQL = ((SAPbouiCOM.DataTable)(this.UIAPIRawForm.DataSources.DataTables.Item("DT_SQL")));

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
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_4").Specific));
            this.Grid0.ValidateAfter += new SAPbouiCOM._IGridEvents_ValidateAfterEventHandler(this.Grid0_ValidateAfter);

            this.Grid0.Item.FromPane = 1;
            this.Grid0.Item.ToPane = 1;

            var sql = @"select * from SBO_COMERCIAL.dbo.[@ZPARAMFA1]";
            DT_SQL.ExecuteQuery(sql);
            Grid0.DataTable = DT_SQL;

        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Folder Folder0;
        private SAPbouiCOM.DataTable DT_SQL;
        private SAPbouiCOM.Grid Grid0;

        private void Grid0_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

        }
    }
}
