using FuncionalidadesAdicionales._1_Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionalidadesAdicionales._2_Business_layer
{
    public static class NVerificaAgregaUDO
    {
        public static bool VerificarCrearUDO()
        {
            bool bExiste = false;

            try
            {
                if (!FuncionesUDO.CheckUDOExists("ZAUTORI"))
                {
                    if (!FuncionesUDT.CheckTableExists("@ZAUTORI"))
                    {
                        CreaUDT_ControlAutorizaciones();
                        FuncionesUDO.CreateUDO("ZAUTORI", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                    }
                    else
                        FuncionesUDO.CreateUDO("ZAUTORI", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                }
                else
                    bExiste = true;

                if (!FuncionesUDO.CheckUDOExists("ZANEXOS"))
                {
                    if (!FuncionesUDT.CheckTableExists("@ZANEXOS"))
                    {
                        CreaUDT_ImportacionAnexos();
                        FuncionesUDO.CreateUDO("ZANEXOS", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                    }
                    else
                        FuncionesUDO.CreateUDO("ZANEXOS", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                }
                else
                    bExiste = true;
            }
            catch (Exception) { }

            return bExiste;
        }

        public static void CreaUDT_ControlAutorizaciones()
        {
            try
            {
                FuncionesUDT.CreateUDT("ZAUTORI", "Tabla Para Control Circ/Aprob", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                FuncionesUDT.CreateUDF("ZAUTORI", "ObjType", "Tipo de Objeto", SAPbobsCOM.BoFieldTypes.db_Alpha, 25, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "DocEntry", "Numero Interno", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "DocNum", "Numero Documento", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "Approved", "Aprobado", SAPbobsCOM.BoFieldTypes.db_Alpha, 10, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "UserPrevDoc", "Aprobador Previo", SAPbobsCOM.BoFieldTypes.db_Alpha, 10, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "UserSing", "Usuario Creador", SAPbobsCOM.BoFieldTypes.db_Alpha, 10, "");
                FuncionesUDT.CreateUDF("ZAUTORI", "CreateDate", "Fecha Creacion", SAPbobsCOM.BoFieldTypes.db_Alpha, 25, "");

            }
            catch (Exception) { }
        }

        public static void CreaUDT_ImportacionAnexos()
        {
            try
            {
                FuncionesUDT.CreateUDT("ZANEXOS", "Temporal para Import. Anexos", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                FuncionesUDT.CreateUDF("ZANEXOS", "ObjType", "Tipo de Objeto", SAPbobsCOM.BoFieldTypes.db_Alpha, 25, "");
                FuncionesUDT.CreateUDF("ZANEXOS", "FormID", "ID Interno", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "");
                FuncionesUDT.CreateUDF("ZANEXOS", "BaseRef", "Numero Origen", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "");
                FuncionesUDT.CreateUDF("ZANEXOS", "BaseType", "Tipo Origen", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "");
                FuncionesUDT.CreateUDF("ZANEXOS", "BaseEntry", "DocEntry Origen", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "");
            }
            catch (Exception) { }
        }
    }

}
