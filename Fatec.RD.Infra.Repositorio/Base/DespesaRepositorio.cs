using Dapper;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Dominio.ViewModel;
using Fatec.RD.Infra.Repositorio.Contexto;

using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Fatec.RD.Infra.Repositorio.Base
{
    public sealed class DespesaRepositorio
    {
        readonly DapperContexto _db;
        readonly IDbConnection _connection;

        public DespesaRepositorio()
        {
            _db = new DapperContexto();
            _connection = _db.Connection;
        }

        /// <summary>
        /// Método que seleciona uma despesa pelo Id
        /// </summary>
        /// <param name="id">Id da despesa</param>
        /// <returns>Objeto de despesa</returns>
        public Despesa SelecionarPorId(int id)
        {
            //td.Descricao AS [TipoDespesa], d.IdTipoPagamento, tp.Descricao AS [TipoPagamento]
            var sqlCommand = @"SELECT d.Id, d.IdTipoPagamento, tp.Descricao as TipoPagamento, d.IdTipoDespesa, td.Descricao as TipoDespesa, d.Valor, d.Comentario, d.Data, d.DataCriacao
                                    FROM Despesa d
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id
                                        WHERE d.Id = @Id";
            //@"SELECT *
            //                        FROM Despesa
            //                            WHERE id = @id";

            return _connection.Query<Despesa>(sqlCommand, new { Id = id }).FirstOrDefault();
        }

        /// <summary>
        /// Método que seleciona uma lista de despesas
        /// </summary>
        /// <returns>Lista de despesas</returns>
        public List<DespesaViewModel> Selecionar()
        {
            var sqlCommand = @"SELECT d.Id, td.Descricao as TipoDespesa, tp.Descricao as TipoPagamento, d.Data, d.Valor, d.Comentario, d.DataCriacao
                                    FROM Despesa d
                                        INNER JOIN TipoPagamento tp ON d.IdTipoPagamento = tp.Id
                                        INNER JOIN TipoDespesa td ON d.IdTipoDespesa = td.Id";

            return _connection.Query<DespesaViewModel>(sqlCommand).ToList();
        }

        /// <summary>
        /// Método que insere uma despesa
        /// </summary>
        /// <param name="obj">Objeto de despesa</param>
        /// <returns>Id da despesa</returns>
        public int Inserir(Despesa obj)
        {
            return _connection.Query<int>(@"INSERT Despesa (IdTipoDespesa, IdTipoPagamento, Valor, Comentario, Data, DataCriacao)
                                                    VALUES (@IdTipoDespesa, @IdTipoPagamento, @Valor, @Comentario, @Data, @DataCriacao)
                                                        SELECT CAST (SCOPE_IDENTITY() as int)", obj).First();
        }

        /// <summary>
        /// Método que altera uma despesa
        /// </summary>
        /// <param name="obj">Objeto de despesa</param>
        /// <returns>Id da despesa</returns>
        public void Alterar(Despesa obj)
        {
            var sqlCommand = @"UPDATE Despesa
                                SET IdTipoDespesa = @IdTipoDespesa, IdTipoPagamento = @IdTipoPagamento, Valor = @Valor, Comentario = @Comentario, Data = @Data
                                WHERE Id = @Id";
            _connection.Execute(sqlCommand, obj);
        }

        /// <summary>
        /// Método que apaga uma despesa
        /// </summary>
        /// <param name="obj">Objeto de despesa</param>
        /// <returns>Id da despesa</returns>
        public void Delete(int id)
        {
            _connection.Execute("DELETE FROM Depesa WHERE Id = @Id", new { Id = id });
        }
    }
}
