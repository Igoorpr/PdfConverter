namespace ConversorPDF.Classes
{
    class ClsExtrairDadosOCR
    {
        private string strProtocolo;
        private string strCpfOuCnpj;
        private int intContadorPagina;

        // Construtor
        public ClsExtrairDadosOCR(string strCpfOuCnpj, int intContadorPagina)
        {
            CpfOuCnpj = strCpfOuCnpj;
            ContadorPagina = intContadorPagina;
        }

        // Getters e setters
        public string CpfOuCnpj
        {
            get
            {
                return strCpfOuCnpj;
            }

            set
            {
                strCpfOuCnpj = value;
            }
        }

        public int ContadorPagina
        {
            get
            {
                return intContadorPagina;
            }

            set
            {
                intContadorPagina = value;
            }
        }

    }

}
