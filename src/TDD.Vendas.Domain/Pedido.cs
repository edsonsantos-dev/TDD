namespace TDD.Vendas.Domain;

public class Pedido
{
    public decimal ValorTotal { get; private set; }
    public PedidoStatus Status { get; private set; }
    public Guid ClienteId { get; private set; }

    private readonly List<PedidoItem> _pedidoItems = new();
    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;


    protected Pedido() { }


    public void AtualizarValorTotal()
    {
        ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
    }

    public void AdicionarItemPedido(PedidoItem pedidoItem)
    {
        if (_pedidoItems.Any(p => p.Id == pedidoItem.Id))
        {
            var pedidoItemExistente = _pedidoItems.FirstOrDefault(p => p.Id == pedidoItem.Id);

            pedidoItemExistente.AdicionarQuantidade(pedidoItem.Quantidade);

            pedidoItem = pedidoItemExistente;

            _pedidoItems.Remove(pedidoItemExistente);
        }

        _pedidoItems.Add(pedidoItem);
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
        Id = id;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }


    internal void AdicionarQuantidade(int quantidade) => Quantidade += quantidade;

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