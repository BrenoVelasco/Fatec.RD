using Dapper;
using Fatec.RD.Dominio.ViewModel;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.Repositorio;
using Fatec.RD.Infra.Repositorio.Contexto;

using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Fatec.RD.Infra.Repositorio.Base
{
    public sealed class DespesaRepositorio //: IRepositorioBase<Despesa>
    {
        readonly DapperContexto _db;
        readonly IDbConnection _connection;

        public DespesaRepositorio()
        {
            _db = new DapperContexto();
            _connection = _db.Connection;
        }

        /// <summary>
        /// Método para alterar uma despesa
        /// </summary>
        /// <param name="obj">Objeto de Despesa</param>
        public void Alterar(Despesa obj)
        {
            var sqlCommand = @"UPDATE Despesa 
                                      SET IdTipoDespesa = @IdTipoDespesa, IdTipoPagamento = @IdTipoPagamento, Data=@Data, 
                                          Valor = @Valor, Comentario = @Comentario
                                        WHERE Id = @Id";

            _connection.Execute(sqlCommand, obj);
        }

        /// <summary>
        /// Método que deleta uma despesa
        /// </summary>
        /// <param name="id">Id da despesa</param>
        public void Delete(int id)
        {
            _connection.Execute("DELETE FROM Despesa WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Método que insere uma despesa
        /// </summary>
        /// <param name="obj">Objeto de despesa</param>
        /// <returns>Id da despesa</returns>
        public int Inserir(Despesa obj)
        {
            return _connection.Query<int>(@"INSERT Despesa (IdTipoDespesa, IdTipoPagamento, Data, Valor, Comentario, DataCriacao)
                                                    VALUES (@IdTipoDespesa, @IdTipoPagamento, @Data, @Valor, @Comentario, @DataCriacao)
                                                        SELECT CAST (SCOPE_IDENTITY() as int)", obj).First();
        }

        /// <summary>
        /// Método que seleciona uma lista de despesas
        /// </summary>
        /// <returns>Lista de despesas</returns>
        public List<DespesaViewModel> Selecionar()
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento, d.Data, d.Valor, d.Comentario
                                    FROM Despesa d
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id";

            return _connection.Query<DespesaViewModel>(sqlCommand).ToList();
        }

        /// <summary>
        /// Método que seleciona uma despesa
        /// </summary>
        /// <param name="id">id da despesa</param>
        /// <returns>Objeto de despesa</returns>
        public Despesa SelecionarPorId(int id)
        {
            var sqlCommand = @"SELECT d.Id, d.IdTipoDespesa, d.IdTipoPagamento, d.Data, d.Valor, d.Comentario, d.DataCriacao, tp.Descricao AS [TipoPagamento], td.Descricao AS [TipoDespesa]
                                    FROM Despesa d
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id
                                        WHERE d.Id = @id";

            return _connection.Query<Despesa>(sqlCommand, new { id }).FirstOrDefault();
        }
    }
}
