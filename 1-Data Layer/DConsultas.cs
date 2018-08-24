using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FuncionalidadesAdicionales._1_Data_Layer;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class DConsultas
    {
        public DConsultas()
        {
        }

        public SAPbobsCOM.Recordset ConsultarArchivoImagenArticuloBEAS(string ItemCode)
        {

            if (!Conexion.oCompany.Connected)
                Conexion.Conectar_Aplicacion();

            SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)Conexion.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string sql = @"SELECT 
	                                ItemCode
	                                ,ItemName
	                                ,PicturName
	                                ,U_picture2
	                                ,U_picture3
	                                ,BitmapPath
                                FROM 
	                                [SBO_INDUSTRIAL].[DBO].[OITM] CROSS JOIN [SBO_INDUSTRIAL].[DBO].[OADP]
                                WHERE 
	                                ItemCode =  '" + ItemCode + "'";
                rs.DoQuery(sql);
            }
            catch (Exception){}
            return rs;
        }

    }
}
