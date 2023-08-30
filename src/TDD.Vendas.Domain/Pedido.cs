using TDD.Core.DomainObjects;

namespace TDD.Vendas.Domain;

public class Pedido
{
    public static int MAX_UNIDADES_ITEM => 15;
    public static int MIN_UNIDADES_ITEM => 1;

    public decimal ValorTotal { get; private set; }
    public PedidoStatus Status { get; private set; }
    public Guid ClienteId { get; private set; }

    private readonly List<PedidoItem> _pedidoItems = new();
    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;


    protected Pedido() { }

    private bool PedidoItemExistente(PedidoItem pedidoItem, out PedidoItem pedidoItemExistente)
    {
        pedidoItemExistente = _pedidoItems.FirstOrDefault(p => p.Id == pedidoItem.Id);

        return pedidoItemExistente != null;
    }

    private PedidoItem ValidarPedidoItemInexistente(PedidoItem pedidoItem)
    {
        if (!PedidoItemExistente(pedidoItem, out var pedidoItemExistente))
            throw new DomainException("Item não existe no pedido");

        return pedidoItemExistente;
    }

    private void AtualizarValorTotal()
    {
        ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
    }

    public void AdicionarItemPedido(PedidoItem pedidoItem)
    {
        if (PedidoItemExistente(pedidoItem, out var pedidoItemExistente))
        {
            pedidoItemExistente.AdicionarQuantidade(pedidoItem.Quantidade);

            pedidoItem = pedidoItemExistente;

            _pedidoItems.Remove(pedidoItemExistente);
        }

        _pedidoItems.Add(pedidoItem);
        AtualizarValorTotal();
    }

    public void AtualizarItemPedido(PedidoItem pedidoItem)
    {
        var pedidoItemExistente = ValidarPedidoItemInexistente(pedidoItem);

        pedidoItem.ValidarQuantidadePermitida(pedidoItem.Quantidade);

        _pedidoItems.Remove(pedidoItemExistente);

        _pedidoItems.Add(pedidoItem);

        AtualizarValorTotal();
    }

    public void RemoverItemPedido(PedidoItem pedidoItem)
    {
        var pedidoItemExistente = ValidarPedidoItemInexistente(pedidoItem);

        _pedidoItems.Remove(pedidoItemExistente);

        AtualizarValorTotal();
    }

    public void TornarRascunho()
    {
        Status = PedidoStatus.Rascunho;
    }

    public static class PedidoFactory
    {
        public static Pedido NovoPedidoRascunho(Guid clienteId)
        {
            var pedido = new Pedido
            {
                ClienteId = clienteId
            };

            pedido.TornarRascunho();

            return pedido;
        }
    }
}

public class PedidoItem
{
    public Guid Id { get; private set; }
    public string NomeProduto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }


    public PedidoItem(Guid id, string nomeProduto, int quantidade, decimal valorUnitario)
    {
        ValidarQuantidadePermitida(quantidade);

        Id = id;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }

    internal void ValidarQuantidadePermitida(int quantidade)
    {
        if (quantidade > Pedido.MAX_UNIDADES_ITEM)
            throw new DomainException($"O máximo de unidades permitida por produto é {Pedido.MAX_UNIDADES_ITEM}");

        if (quantidade < Pedido.MIN_UNIDADES_ITEM)
            throw new DomainException($"O pedido deve ter no mínimo {Pedido.MIN_UNIDADES_ITEM} produto");
    }

    internal void AdicionarQuantidade(int quantidade) => ValidarQuantidadePermitida(Quantidade += quantidade);

    internal decimal CalcularValor() => Quantidade * ValorUnitario;
}

public enum PedidoStatus
{
    Rascunho = 100,
    Iniciado = 200,
    Pago = 300,
    Entregue = 400,
    Cancelado = 500
}