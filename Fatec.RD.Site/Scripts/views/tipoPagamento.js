﻿/*********** Declarando variáveis para facilitar ***************/
var api = "http://localhost:63649/api/TipoPagamento/";
var formulario = $("#form-tipoPagamento");
var body = $("#modal-tipoPagamento");
var titleModal = $("#modal-tipoPagamento #modal-title");
var botaoSalvar = $("#salvar-tipoPagamento");
var tabela = $("#tabela-tipoPagamento");

var descricao = $("#descricao");

var t = tabela.mDatatable({
    translate: {
        records: {
            noRecords: "Nenhum resultado encontrado.",
            processing: "Processando..."
        },
        toolbar: {
            pagination: {
                items: {
                    default: {
                        first: "Primeira",
                        prev: "Anterior",
                        next: "Próxima",
                        last: "Última",
                        more: "Mais",
                        input: "Número da página",
                        select: "Selecionar tamanho da página"
                    },
                    info: 'Exibindo' + ' {{start}} - {{end}} ' + 'de' + ' {{total}} ' + 'resultados'
                }
            }
        }
    },
    data: {
        type: "remote",
        source: {
            read: {
                method: "GET",
                url: api,
                map: function (t) {
                    var e = t;
                    return void 0 !== t.data && (e = t.data), e;
                }
            }
        },
        pageSize: 10,
        serverPaging: !0,
        serverFiltering: !0,
        serverSorting: !0
    },
    layout: {
        theme: "default",
        class: "",
        scroll: !1,
        footer: !1
    },
    sortable: !0,
    pagination: !0,
    toolbar: {
        items: {
            pagination: {
                pageSizeSelect: [10, 20, 30, 50, 100]
            }
        }
    },
    search: {
        input: $("#generalSearch")
    },
    columns: [
        {
            field: "Descricao",
            title: "Descrição",
            filterable: true
        },
        {
            field: "Acoes",
            title: "Ações",
            width: 50,
            sortable: false,
            overflow: "visible",
            template: function (t, e, a) {
                return '\
                            <div class="dropdown">\
                                <a href="#" class="btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill" data-toggle="dropdown">\
                                    <i class="la la-ellipsis-h"></i>\
                                </a>\
                                <div class="dropdown-menu dropdown-menu-right">\
                                    <a class="dropdown-item" href="#" onclick="abrirModalAlterar(' + t.Id + ')">\
                                        <i class="la la-edit"></i> Editar\
                                    </a>\
                                    <a class="dropdown-item" href="#" onclick="abrirModalExcluir('+ t.Id + ')">\
                                        <i class="la la-leaf"></i> Excluir\
                                    </a>\
                                </div>\
                            </div>';
            }
        }]
});

/***************************************************************/


/************* Funções disparadas nos cliques ******************/
function novoTipoPagamento() {
    formValidation();
    titleModal.html("Adicionar Tipo de Pagamento");
    body.modal('show');
}

function abrirModalAlterar(id) {
    formValidation();
    botaoSalvar.attr("data-id", id);
    var tipoPagamento = selecionarPorId(id);
    descricao.val(tipoPagamento.Descricao);
    titleModal.html("Alterar Tipo de Relatório");
    body.modal('show');
}

function abrirModalExcluir(id) {
    if (confirm("Tem certeza que deseja excluir?")) {
        excluir(id);
    }
}

function fechar() {
    formulario.validate().destroy();
}
/***************************************************************/

/************** Funções com requisições para a API *************/
function inserir(tipoPagamento) {
    console.log(tipoPagamento);
    $.ajax({
        type: 'POST',
        url: api,
        data: JSON.stringify(tipoPagamento),
        contentType: 'application/json',
        success: function () {
            alert("Inserido com sucesso!");
            t.reload();
            body.modal('hide');
            limparCampos();
        },
        error: function (error) {
            alert(error.responseJSON.error);
        }
    });
}

function alterar(id, tipoPagamento) {
    $.ajax({
        type: 'PUT',
        url: api + '/' + id,
        data: JSON.stringify(tipoPagamento),
        contentType: 'application/json',
        success: function () {
            alert("Alterado com sucesso!");
            t.reload();
            body.modal('hide');
            limparCampos();
        },
        error: function (error) {
            alert(error.responseJSON.error);
        }
    });
}

function excluir(id) {

    $.ajax({
        type: 'DELETE',
        url: api + '/' + id,
        success: function () {
            alert("Deletado com sucesso!");
            t.reload();
        },
        error: function (error) {
            console.log(error);
        }
    });

}

function selecionarPorId(id) {
    $.ajaxSetup({ async: false }); // Força com que ela espere o retorno para prosseguir - Assim consigo pegar o resultado antes dele abrir o modal
    var retorno;
    $.ajax({
        type: 'GET',
        url: api + '/' + id,
        success: function (tipoPagamento) {
            retorno = tipoPagamento;
        },
        error: function (error) {
            console.log(error);
        }
    });
    $.ajaxSetup({ async: true });
    return retorno;
}
/***************************************************************/

/*********************** Funções internas **********************/


function salvarTipoPagamento() {
    var id = botaoSalvar.attr("data-id");
    var tipoPagamento = {
        Descricao: descricao.val()
    };

    if (id !== undefined)
        alterar(id, tipoPagamento);
    else
        inserir(tipoPagamento);
}

function preencherCampos(tipoPagamento) {
    data.val(tipoPagamento.Data);
    descricao.val(tipoPagamento.Descricao);
}

function limparCampos() {
    formulario.each(function () {
        this.reset();
    });

    botaoSalvar.removeAttr('data-id');
}

/***************************************************************/

/************************* Validações **************************/
function formValidation() {
    formulario.validate({
        errorClass: "errorClass",
        rules: {
            descricao: { required: true }
        },
        messages: {
            descricao: { required: "Campo obrigatório." }
        },
        submitHandler: function (form) {
            salvarTipoPagamento();
        }
    });
}