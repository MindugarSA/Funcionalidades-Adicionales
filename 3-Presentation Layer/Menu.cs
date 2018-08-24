using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;

using FuncionalidadesAdicionales._3_Presentation_Layer;
using FuncionalidadesAdicionales._3_Presentation_Layer.Users_Forms;

namespace FuncionalidadesAdicionales
{
    class Menu
    {
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("3328"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            oCreationPackage.UniqueID = "FuncionalidadesAdicionales";
            oCreationPackage.String = "Parametros Funcionalidades Adicionales";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            if (Application.SBO_Application.Menus.Exists("FuncionalidadesAdicionales") == true)
            {
                Application.SBO_Application.Menus.RemoveEx("FuncionalidadesAdicionales");
            }

            try
            {
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception){}

            try
            {
                //// Get the menu collection of the newly added pop-up item
                //oMenuItem = Application.SBO_Application.Menus.Item("FuncionalidadesAdicionales");
                //oMenus = oMenuItem.SubMenus;

                //// Create s sub menu
                //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                //oCreationPackage.UniqueID = "FuncionalidadesAdicionales.Form1";
                //oCreationPackage.String = "Form1";
                //oMenus.AddEx(oCreationPackage);
            }
            catch 
            { //  Menu already exists
                //Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "FuncionalidadesAdicionales")
                {
                    frmParametrosAdic activeForm = new frmParametrosAdic();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }
    }
}
