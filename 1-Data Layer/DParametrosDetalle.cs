using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionalidadesAdicionales._1_Data_Layer
{
    class DParametrosDetalle
    {
        private String _U_CodFamilia;
        private String _U_Familia;
        private String _U_CantMaestros;
        private String _U_CantAyudantes;
        private String _U_CantOtros1;
        private String _U_CantOtros2;
        private String _U_CantDias;
        private String _U_TotalMts2;
        private String _U_CostoMaestro;
        private String _U_CostoAyudante;
        private String _U_CostoViaticos;


        public virtual String U_CodFamilia
        {
            get
            {
                return this._U_CodFamilia;
            }
            set
            {
                this._U_CodFamilia = value;
            }
        }

        public virtual String U_Familia
        {
            get
            {
                return this._U_Familia;
            }
            set
            {
                this._U_Familia = value;
            }
        }

        public virtual String U_CantMaestros
        {
            get
            {
                return this._U_CantMaestros;
            }
            set
            {
                this._U_CantMaestros = value;
            }
        }

        public virtual String U_CantAyudantes
        {
            get
            {
                return this._U_CantAyudantes;
            }
            set
            {
                this._U_CantAyudantes = value;
            }
        }

        public virtual String U_CantOtros1
        {
            get
            {
                return this._U_CantOtros1;
            }
            set
            {
                this._U_CantOtros1 = value;
            }
        }

        public virtual String U_CantOtros2
        {
            get
            {
                return this._U_CantOtros2;
            }
            set
            {
                this._U_CantOtros2 = value;
            }
        }

        public virtual String U_CantDias
        {
            get
            {
                return this._U_CantDias;
            }
            set
            {
                this._U_CantDias = value;
            }
        }

        public virtual String U_TotalMts2
        {
            get
            {
                return this._U_TotalMts2;
            }
            set
            {
                this._U_TotalMts2 = value;
            }
        }

        public virtual String U_CostoMaestro
        {
            get
            {
                return this._U_CostoMaestro;
            }
            set
            {
                this._U_CostoMaestro = value;
            }
        }

        public virtual String U_CostoAyudante
        {
            get
            {
                return this._U_CostoAyudante;
            }
            set
            {
                this._U_CostoAyudante = value;
            }
        }

        public virtual String U_CostoViaticos
        {
            get
            {
                return this._U_CostoViaticos;
            }
            set
            {
                this._U_CostoViaticos = value;
            }
        }


        public DParametrosDetalle()
        {
        }

        public DParametrosDetalle( String U_CodFamilia
                                  ,String U_Familia
                                  ,String U_CantMaestros
                                  ,String U_CantAyudantes
                                  ,String U_CantOtros1
                                  ,String U_CantOtros2
                                  ,String U_CantDias
                                  ,String U_TotalMts2
                                  ,String U_CostoMaestro
                                  ,String U_CostoAyudante
                                  ,String U_CostoViaticos)
        {
            this.U_CodFamilia    = U_CodFamilia;
            this.U_Familia       = U_Familia;
            this.U_CantMaestros  = U_CantMaestros;
            this.U_CantAyudantes = U_CantAyudantes;
            this.U_CantOtros1    = U_CantOtros1;
            this.U_CantOtros2    = U_CantOtros2;
            this.U_CantDias      = U_CantDias;
            this.U_TotalMts2     = U_TotalMts2;
            this.U_CostoMaestro  = U_CostoMaestro;
            this.U_CostoAyudante = U_CostoAyudante;
            this.U_CostoViaticos = U_CostoViaticos;
        }
    }
}
