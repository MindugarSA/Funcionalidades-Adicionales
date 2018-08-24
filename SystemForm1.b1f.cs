using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace FuncionalidadesAdicionales
{
    [FormAttribute("720", "SystemForm1.b1f")]
    class SystemForm1 : SystemFormBase
    {
        public SystemForm1()
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

        private SAPbouiCOM.Button Button0;

        private void OnCustomInitialize()
        {

        }

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            //SAPbouiCOM.FormCreationParams creationPackage;
            //creationPackage = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);


            //creationPackage.UniqueID = "FuncionalidadesAdicionales_99901";
            //creationPackage.FormType = "FuncionalidadesAdicionales_UserForm";

            //oForm = SBO_Application.Forms.AddEx(creationPackage)

            Form1 activeForm = new Form1();

            Application.SBO_Application.MessageBox(activeForm.UIAPIRawForm.UniqueID);

            //SAPbouiCOM.UserDataSource oUDS = activeForm.UIAPIRawForm.DataSources.UserDataSources.Item("UD_0");
            //oUDS.ValueEx = pVal.FormUID;

            activeForm.Show();

        }
    }
}
