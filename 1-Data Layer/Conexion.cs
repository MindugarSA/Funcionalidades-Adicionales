using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class Conexion
    {
        public static SAPbouiCOM.Application oApplication;
        public static SAPbobsCOM.Company oCompany;
        public static SAPbobsCOM.SBObob oSBObob;

        public static string sCodUsuActual;
        public static string sAliasUsuActual;
        public static string sNomUsuActual;
        public static string sCurrentCompanyDB;

        public static void Conectar_Aplicacion()
        {


            SAPbouiCOM.SboGuiApi SboGuiApi = new SAPbouiCOM.SboGuiApi();
            oCompany = new SAPbobsCOM.Company();
            SboGuiApi.Connect("0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056");
            oApplication = SboGuiApi.GetApplication();
            oCompany = (SAPbobsCOM.Company)oApplication.Company.GetDICompany();

            oSBObob = (SAPbobsCOM.SBObob)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

            sCodUsuActual = oCompany.UserSignature.ToString();
            sNomUsuActual = Funciones.Current_User_Name();
            sAliasUsuActual = oCompany.UserName;
            sCurrentCompanyDB = oCompany.CompanyDB;
        }
    }
}


