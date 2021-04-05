using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class EnderecoDAO
    {
        private SqlParameter[] CriaParametros(EnderecoViewModel endereco)
        {
            SqlParameter[] p = {
                new SqlParameter("cep", endereco.CEP),
                new SqlParameter("rua", endereco.Street),
                new SqlParameter("bairro", endereco.District),
                new SqlParameter("cidade", endereco.City),
                new SqlParameter("estado", endereco.State),
                new SqlParameter("numero", endereco.Number),
            };

            return p;
        }

        public void Inserir(EnderecoViewModel endereco)
        {
            string sql = "insert into Endereco (cep, rua, bairro, cidade, estado,numero)" +
                "values (@cep, @rua, @bairro, @cidade, @estado, @numero)";

            HelperDAO.ExecutaSQL(sql, CriaParametros(endereco));
        }

        
        public EnderecoViewModel Consulta(string cep)
        {
            string sql = "select * from Endereco where cep = " + cep;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<EnderecoViewModel> Lista()
        {
            string sql = "select * from Endereco";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<EnderecoViewModel> retorno = new List<EnderecoViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }

        public int ProximoId()
        {
            string sql = "select isnull(max(id) +1, 1) as 'MAIOR' from Endereco";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }

        public static EnderecoViewModel MontaModel(DataRow registro)
        {
            EnderecoViewModel main = new EnderecoViewModel();
            main.CEP = Convert.ToInt32(registro["cep"]);
            main.Street = registro["rua"].ToString();
            main.District = registro["bairro"].ToString();
            main.City = registro["cidade"].ToString();
            main.State = registro["estado"].ToString();
            main.Number = Convert.ToInt32(registro["numero"]);
            return main;
        }
    }
}
