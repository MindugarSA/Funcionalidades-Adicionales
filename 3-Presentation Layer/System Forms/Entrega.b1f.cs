using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._2_Business_layer;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("140", "3-Presentation Layer/System Forms/Entrega.b1f")]
    class SystemForm4 : SystemFormBase
    {
        public SystemForm4()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
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
            Button0.Item.Visible = false;
        }

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Application.SBO_Application.ActivateMenuItem("5921");

        }

        private SAPbouiCOM.Button Button0;

    }
}
