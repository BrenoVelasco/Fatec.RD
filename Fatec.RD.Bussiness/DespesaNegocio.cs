using Fatec.RD.Bussiness.Inputs;
using Fatec.RD.Dominio.Modelos;
using Fatec.RD.Infra.Repositorio.Base;
using Fatec.RD.SharedKernel.Excecoes;
using System;


using Fatec.RD.Dominio.ViewModel;
using System.Collections.Generic;


namespace Fatec.RD.Bussiness
{
    public sealed class DespesaNegocio
    {
        DespesaRepositorio _despesaRepositorio;
        TipoDespesaRepositorio _tipoDespesaRepositorio;
        TipoPagamentoRepositorio _tipoPagamentoRepositorio;


        public DespesaNegocio()
        {
            _despesaRepositorio = new DespesaRepositorio();
            _tipoDespesaRepositorio = new TipoDespesaRepositorio();
            _tipoPagamentoRepositorio = new TipoPagamentoRepositorio();
        }

        /// <summary>
        /// Método que seleciona uma lista de despesas
        /// </summary>
        /// <returns></returns>
        public List<DespesaViewModel> Selecionar()
        {
            return _despesaRepositorio.Selecionar();
        }

        /// <summary>
        /// Método que seleciona despesas por Id
        /// </summary>
        /// <returns></returns>
        public Despesa SelecionarPorId(int id)
        {
            var retorno = _despesaRepositorio.SelecionarPorId(id);

            if (retorno.Id <= 0)
                throw new NaoEncontradoException("Despesa não encontrada", id);

            return retorno;
        }


        /// <summary>
        /// Método que adiciona uma despesa
        /// </summary>
        /// <param name="obj">Objeto de Despesa</param>
        /// <returns>Uma nova despesa</returns>
        public Despesa Adicionar(DespesaInput obj)
        {

            var novoObj = new Despesa()
            {
                IdTipoDespesa=obj.IdTipoDespesa,
                IdTipoPagamento=obj.IdTipoPagamento,
                Valor = obj.Valor,
                Comentario = obj.Comentario,
                Data=obj.Data,
                DataCriacao = DateTime.Now
            };

            novoObj.Validar();

            var retorno = _despesaRepositorio.Inserir(novoObj);

            return _despesaRepositorio.SelecionarPorId(retorno);
        }

        /// <summary>
        /// Método que altera uma despesa
        /// </summary>
        /// <param name="id">Id da depsesa</param>
        /// <param name="input">Objeto de input de despesa</param>
        /// <returns>Objeto de despesa</returns>
        public Despesa Alterar(int id, DespesaInput input)
        {
            var obj = this.SelecionarPorId(id);

            obj.IdTipoDespesa = input.IdTipoDespesa;
            obj.IdTipoPagamento = input.IdTipoPagamento;
            obj.Valor = input.Valor;
            obj.Comentario = input.Comentario;
            obj.Data = input.Data;
            obj.Validar();

            _despesaRepositorio.Alterar(obj);

            return obj;
        }

        /// <summary>
        /// Método que deleta uma despesa
        /// </summary>
        /// <param name="id">Id do tipo de despesa</param>
        public void Deletar(int id)
        {
            try
            {
                var obj = this.SelecionarPorId(id);
                _despesaRepositorio.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Aconteceu um erro ao deletar. Talvez a despesa não exista.");
            }
        }

    }
}
