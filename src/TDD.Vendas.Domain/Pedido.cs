namespace TDD.Vendas.Domain;

public class Pedido
{
    public decimal ValorTotal { get; set; }
    
    private readonly List<PedidoItem> _pedidoItems = new();
    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

    public void AdicionarItemPedido(PedidoItem pedidoItem)
    {
        _pedidoItems.Add(pedidoItem);
        ValorTotal = PedidoItems.Sum(p => p.Valor * p.Quantidade);
    }
}

public class PedidoItem
{
    public Guid Id { get; private set; }
    public string NomeProduto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal Valor { get; private set; }

    public PedidoItem(Guid id, string nomeProduto, int quantidade, decimal valor)
    {
        Id = id;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        Valor = valor;
    }
}