using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// Biblioteca Conversor pdf
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ConversorPDF.Classes;

// Bibliotecas Excel
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ConversorPDF
{
    public partial class Principal_Form : Form
    {
        public Principal_Form()
        {
            InitializeComponent();
        }

        // Lista para armazenar os dados Filtrados
        List<ClsExtrairDadosOCR> lstDadosExtraidos = new List<ClsExtrairDadosOCR>();

        // Lista para gravar os dados (CPF e CNPJ)  
        List<string> lstCpfeCnpjFiltrados = new List<string>();

        #region ConverterPdf
        public List<string> TransformarPdfEmTexto(string strNomeArquivo)
        {
            List<string> lstPaginas = new List<string>();

            try
            {
                PdfReader objLeitura = new PdfReader(strNomeArquivo);

                StringBuilder objTexto = new StringBuilder();

                string[] strmatLinhas;

                for (int i = 1; i <= objLeitura.NumberOfPages; i++)
                {
                    strmatLinhas = PdfTextExtractor.GetTextFromPage(objLeitura, i).Split('\n');

                    foreach (string strLinhas in strmatLinhas)
                    {
                        objTexto.AppendLine(strLinhas);
                    }

                    lstPaginas.Add(RetirarAcentos(objTexto.ToString().ToUpper()) + "\n");
                    objTexto.Clear();
                }
                
            } catch
            {
                MessageBox.Show("Problema com o PDF! Tente novamente com outro PDF!");
            }
            return lstPaginas;
        }

        public string RetirarAcentos(string strTexto)
        {
            var Codificar8Bits = Encoding.GetEncoding(1251).GetBytes(strTexto);
            var String7Bits = Encoding.ASCII.GetString(Codificar8Bits);
            var RetiraAcento = new Regex("[^a-zA-Z0-9]=-_/");
            String strResultado = RetiraAcento.Replace(String7Bits, " ");

            return strResultado;
        }
        #endregion

        // Retornará apenas numeros
        public string RetornarNumeros(string strTexto)
        {
            var ApenasDigitos = new Regex(@"[^\d]");
            return (ApenasDigitos.Replace(strTexto, ""));
        }

        #region Localizar Dados
        public void FiltrarDados(List<string> lstListarPaginas)
        {
            int intContadorPagina = 0;
        
            List<string> lstAuxiliarCpfOuCnpj = new List<string>();
            List<string> lstArmazenarObjCpfeCnpj = new List<string>();

            foreach (string strPercorrerLista in lstListarPaginas)
            {
                lstAuxiliarCpfOuCnpj = FiltrarCpfeCnpj(strPercorrerLista);

                // foreach para validar cpf e cnpj de dados e armazenar nas listas 
                foreach (string strCpfOuCnpj in lstAuxiliarCpfOuCnpj)
                {
                    if ((ValidaCpf(strCpfOuCnpj) == true))
                    {
                        lstCpfeCnpjFiltrados.Add(strCpfOuCnpj);
                        lstArmazenarObjCpfeCnpj.Add(strCpfOuCnpj);
                    }

                    if (ValidaCnpj(strCpfOuCnpj) == true)
                    {
                        lstCpfeCnpjFiltrados.Add(strCpfOuCnpj);
                        lstArmazenarObjCpfeCnpj.Add(strCpfOuCnpj);
                    }
                }

                intContadorPagina++;

                ArmazenarLista(lstArmazenarObjCpfeCnpj, intContadorPagina);

                lstArmazenarObjCpfeCnpj.Clear();
                lstAuxiliarCpfOuCnpj.Clear();
            } // end foreach
        }
        #endregion

        #region Filtragem CPF e CNPJ
        public List<string> FiltrarCpfeCnpj(string strTextoPagina)
        {
            List<string> lstAuxiliarCpf = new List<string>();

            string strAuxiliar = "", strSemCharEspeciais, strFinal = "";

            strAuxiliar = Regex.Replace(strTextoPagina, @"\s", "");

            strSemCharEspeciais = Regex.Replace(strAuxiliar, @"[\""]", "");

            var regex = new Regex(@"([0-9]{2}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[\\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\\.]?[0-9]{3}[\\.]?[0-9]{3}[-]?[0-9]{2})");

            string[] strmatCpfeCnpj;
            
            foreach (Match match in regex.Matches(strSemCharEspeciais))
            {
                strFinal += match.ToString() + ";";
            }

            strmatCpfeCnpj = strFinal.Split(';');

            foreach (string strCpfOuCnpj in strmatCpfeCnpj)
            {
                lstAuxiliarCpf.Add(strCpfOuCnpj);
            }

            return lstAuxiliarCpf;
        }
        #endregion

        #region Valida CPF e CNPJ
        public bool ValidaCpf(string strCpf)
        {
            strCpf = RetornarNumeros(strCpf);

            if (strCpf.Length > 11)
                return false;

            while (strCpf.Length != 11)
                strCpf = '0' + strCpf;

            bool bolIgual = true;
            for (int intContador = 1; intContador < 11 && bolIgual; intContador++)
                if (strCpf[intContador] != strCpf[0])
                    bolIgual = false;

            if (bolIgual || strCpf == "12345678909")
                return false;

            int[] intmatNumeros = new int[11];

            for (int intContador = 0; intContador < 11; intContador++)
                intmatNumeros[intContador] = int.Parse(strCpf[intContador].ToString());

            int intSomaNumeros = 0;
            for (int intContador = 0; intContador < 9; intContador++)
                intSomaNumeros += (10 - intContador) * intmatNumeros[intContador];

            int intResultado = intSomaNumeros % 11;

            if (intResultado == 1 || intResultado == 0)
            {
                if (intmatNumeros[9] != 0)
                    return false;
            }
            else if (intmatNumeros[9] != 11 - intResultado)
                return false;

            intSomaNumeros = 0;
            for (int i = 0; i < 10; i++)
                intSomaNumeros += (11 - i) * intmatNumeros[i];

            intResultado = intSomaNumeros % 11;

            if (intResultado == 1 || intResultado == 0)
            {
                if (intmatNumeros[10] != 0)
                    return false;
            }
            else
                if (intmatNumeros[10] != 11 - intResultado)
                return false;

            return true;
        }

        public bool ValidaCnpj (string strCpfOuCnpj)
        {
            
            int[] matMultiplicador = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] matMultiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int intSoma = 0, intResto;

            string strDigito, strTempCnpj = "";

            strCpfOuCnpj = strCpfOuCnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            if (strCpfOuCnpj.Length != 14)
                return false;

            if (!strCpfOuCnpj.Contains("0001"))
                return false;

            if (strCpfOuCnpj.Contains("5000001"))
                return false;

            strTempCnpj = strCpfOuCnpj.Substring(0, 12);

            for (int i = 0; i < 12; i++)
                intSoma += int.Parse(strTempCnpj[i].ToString()) * matMultiplicador[i];
                intResto = (intSoma % 11);

            if (intResto < 2)
                intResto = 0;
            else
                intResto = 11 - intResto;
                strDigito = intResto.ToString();
                strTempCnpj = strTempCnpj + strDigito;

            intSoma = 0;

            for (int i = 0; i < 13; i++)
                intSoma += int.Parse(strTempCnpj[i].ToString()) * matMultiplicador2[i];
                intResto = (intSoma % 11);

            if (intResto < 2)
                intResto = 0;
            else
                intResto = 11 - intResto;
                strDigito = strDigito + intResto.ToString();

            return strCpfOuCnpj.EndsWith(strDigito); 
        }
        #endregion
 
        public void ArmazenarLista(List<string> lstAuxiliarCpfOuCnpj, int intContadorPagina)
        {
            ClsExtrairDadosOCR objDadosExtraidos = null;

            if (lstAuxiliarCpfOuCnpj.Count == 0)
            {
                lstAuxiliarCpfOuCnpj.Add("-");
            }

            foreach (string strPercorreListaCpfOuCnpj in lstAuxiliarCpfOuCnpj)
            {
                objDadosExtraidos = new ClsExtrairDadosOCR(strPercorreListaCpfOuCnpj, intContadorPagina);
                lstDadosExtraidos.Add(objDadosExtraidos);

            } // fim foreach strPercorreListaCpfOuCnpj
        }

        #region Impressão das Listas
        // Imprimir dados finais arquivo (Testes)
        public void ImprimirDadosListaObj(string strNomeTxt)
        {
            StreamWriter escreverarquivo = new StreamWriter(@"C:\Users\PC\Documents\Path\Dados.txt");

            try
            {
                IWorkbook objDadosExcel = new XSSFWorkbook();
                ISheet objNomePlanilha = objDadosExcel.CreateSheet("Planilha");

                IRow objCabecalho = objNomePlanilha.CreateRow(0);
                objCabecalho.CreateCell(0).SetCellValue("ARQUIVO");
                objCabecalho.CreateCell(2).SetCellValue("CPF/CNPJ");
                objCabecalho.CreateCell(3).SetCellValue("PÁGINA");

                int intPosicaoExcel = 1;

                foreach (ClsExtrairDadosOCR objPercorrerLista in lstDadosExtraidos)
                {
                    escreverarquivo.WriteLine("DADOS EXTRAIDOS DA PÁGINA: ");
                    escreverarquivo.WriteLine("Cpf/Cnpj: " + objPercorrerLista.CpfOuCnpj);
                    escreverarquivo.WriteLine("localizado na Página: " + objPercorrerLista.ContadorPagina);
                    escreverarquivo.WriteLine("\n");

                    IRow objLinha = objNomePlanilha.CreateRow(intPosicaoExcel);
                    objLinha.CreateCell(2).SetCellValue(objPercorrerLista.CpfOuCnpj);
                    objLinha.CreateCell(3).SetCellValue(objPercorrerLista.ContadorPagina);

                    intPosicaoExcel++;
                }

                FileStream objCriaExcel = File.Create(@"C:\Users\PC\Documents\Path\EXCEL.xlsx");
                objDadosExcel.Write(objCriaExcel);
                objCriaExcel.Close();
            }
            catch
            {
                MessageBox.Show("Problema com Excel");
            }
            escreverarquivo.Close();
        }

        // Apenas para testes 
        public void CriaArquivoTXT(List<string> pagina)
        {
            String Texto_Convertido = "";

            StreamWriter escreverarquivo = new StreamWriter(@"C:\Users\PC\Documents\Path\CONVERSAO.txt");
            Texto_Convertido = string.Join(string.Empty, pagina.ToArray());
            escreverarquivo.WriteLine(Texto_Convertido);
            escreverarquivo.Close();
        }
        #endregion

        private void Converter_bt_Click(object sender, EventArgs e)
        {
            #region ConverterPDF
            String strNomePdf = " ", strNomeTxt = "";

            strNomePdf = NomePDF_TB.Text + ".pdf";

            strNomeTxt = NomePDF_TB.Text + ".txt";

            // Lista com as páginas convertidas (Listapaginas)
            List<string> lstListarPaginas = TransformarPdfEmTexto(strNomePdf);

            // Metodo apenas para testes
            CriaArquivoTXT(lstListarPaginas);
            #endregion

            // Localiza CPF, CNPJ 
            FiltrarDados(lstListarPaginas);

            // Remove da Lista CPF/CNPJ que estão com (-)
            var varProcuraItensLista = from varProcura in lstDadosExtraidos.ToList() where varProcura.CpfOuCnpj == "-" select varProcura;
            foreach (var objPercorrerLista in varProcuraItensLista)
            {
                lstDadosExtraidos.Remove(objPercorrerLista);
            }

            // Cria Arquivo com todos os dados filtrados
            ImprimirDadosListaObj(NomePDF_TB.Text);

            MessageBox.Show("Processo foi finalizado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void fechar_bt_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
