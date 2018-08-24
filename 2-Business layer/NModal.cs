using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncionalidadesAdicionales._2_Business_layer
{
    class NModal
    {
        private static bool _esPantallaModal;
        private static string _IDPantallaModal;

        public static bool esPantallaModal
        {
            get
            {
                return _esPantallaModal;
            }

            set
            {
                _esPantallaModal = value;
            }
        }

        public static string IDPantallaModal
        {
            get
            {
                return _IDPantallaModal;
            }

            set
            {
                _IDPantallaModal = value;
            }
        }
    }
}
