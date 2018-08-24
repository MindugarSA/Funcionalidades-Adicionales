using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class DParametros
    {

        private String _U_Kilo_Flete;
        private String _U_Factor_Comercial;
        private String _U_FleteMinimo;
        private String _U_ParametroNum1;
        private String _U_ParametroNum2;
        private String _U_ParametroNum3;
        private String _U_ParametroNum4;
        private String _U_ParametroText1;
        private String _U_ParametroText2;
        private String _U_ParametroText3;
        private String _U_ParametroText4;
        private String _U_Comentario1;
        private String _U_Comentario2;
        
        public virtual String U_Kilo_Flete
        {
            get
            {
                return this._U_Kilo_Flete;
            }
            set
            {
                this._U_Kilo_Flete = value;
            }
        }
        public virtual String U_Factor_Comercial
        {
            get
            {
                return this._U_Factor_Comercial;
            }
            set
            {
                this._U_Factor_Comercial = value;
            }
        }
        public virtual String U_FleteMinimo
        {
            get
            {
                return this._U_FleteMinimo;
            }
            set
            {
                this._U_FleteMinimo = value;
            }
        }
        public virtual String U_ParametroNum1
        {
            get
            {
                return this._U_ParametroNum1;
            }
            set
            {
                this._U_ParametroNum1 = value;
            }
        }
        public virtual String U_ParametroNum2
        {
            get
            {
                return this._U_ParametroNum2;
            }
            set
            {
                this._U_ParametroNum2 = value;
            }
        }
        public virtual String U_ParametroNum3
        {
            get
            {
                return this._U_ParametroNum3;
            }
            set
            {
                this._U_ParametroNum3 = value;
            }
        }
        public virtual String U_ParametroNum4
        {
            get
            {
                return this._U_ParametroNum4;
            }
            set
            {
                this._U_ParametroNum4 = value;
            }
        }
        public virtual String U_ParametroText1
        {
            get
            {
                return this._U_ParametroText1;
            }
            set
            {
                this._U_ParametroText1 = value;
            }
        }
        public virtual String U_ParametroText2
        {
            get
            {
                return this._U_ParametroText2;
            }
            set
            {
                this._U_ParametroText2 = value;
            }
        }
        public virtual String U_ParametroText3
        {
            get
            {
                return this._U_ParametroText3;
            }
            set
            {
                this._U_ParametroText3 = value;
            }
        }
        public virtual String U_ParametroText4
        {
            get
            {
                return this._U_ParametroText4;
            }
            set
            {
                this._U_ParametroText4 = value;
            }
        }
        public virtual String U_Comentario1
        {
            get
            {
                return this._U_Comentario1;
            }
            set
            {
                this._U_Comentario1 = value;
            }
        }
        public virtual String U_Comentario2
        {
            get
            {
                return this._U_Comentario2;
            }
            set
            {
                this._U_Comentario2 = value;
            }
        }

        public DParametros()
        {
        }

        public DParametros( String U_Kilo_Flete
                           ,String U_Factor_Comercial
                           ,String U_FleteMinimo
                           ,String U_ParametroNum1
                           ,String U_ParametroNum2
                           ,String U_ParametroNum3
                           ,String U_ParametroNum4
                           ,String U_ParametroText1
                           ,String U_ParametroText2
                           ,String U_ParametroText3
                           ,String U_ParametroText4
                           ,String U_Comentario1
                           ,String U_Comentario2)
        {
            this.U_Kilo_Flete       = U_Kilo_Flete ;
            this.U_Factor_Comercial = U_Factor_Comercial;
            this.U_FleteMinimo      = U_FleteMinimo;
            this.U_ParametroNum1    = U_ParametroNum1;
            this.U_ParametroNum2    = U_ParametroNum2;
            this.U_ParametroNum3    = U_ParametroNum3;
            this.U_ParametroNum4    = U_ParametroNum4;
            this.U_ParametroText1   = U_ParametroText1;
            this.U_ParametroText2   = U_ParametroText2;
            this.U_ParametroText3   = U_ParametroText3;
            this.U_ParametroText4   = U_ParametroText4;
            this.U_Comentario1      = U_Comentario1;
            this.U_Comentario2      = U_Comentario2;
        }


    }
}
