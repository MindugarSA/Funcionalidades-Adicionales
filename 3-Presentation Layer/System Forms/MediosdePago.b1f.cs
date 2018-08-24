using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._2_Business_layer;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("196", "3-Presentation Layer/System Forms/MediosdePago.b1f")]
    class SystemForm5 : SystemFormBase
    {
        private static SAPbouiCOM.Form oForm = null;

        public SystemForm5()
        {
        }
        ///-- MODIFICACIONES SOBRE EL FORM --
        /// Para poder distingir cuando un pago emitido contiene una transferencia bancaria, se creo una tabla [@ZAUTORI]
        /// donde al momento de  
        /// 
        /// 
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
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
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
        }

        private SAPbouiCOM.Button Button0;

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
           
        }
    }
}
