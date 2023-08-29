using TDD.Core.DomainObjects;

namespace TDD.Vendas.Domain.Tests;

public class PedidoTests
{
    [Fact(DisplayName = "Adicionar Item Novo Pedido")]
    [Trait("Categoria", "Pedido Tests")]
    public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var pedidoItem = new PedidoItem(Guid.NewGuid(), "Pedido Item Teste", 2, 100);


        //Act
        pedido.AdicionarItemPedido(pedidoItem);


        //Assert
        Assert.Equal(200, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Adicionar Item Pedido Existente")]
    [Trait("Categoria", "Pedido Tests")]
    public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItem = new PedidoItem(produtoId, "Pedido Item Teste", 2, 100);
        pedido.AdicionarItemPedido(pedidoItem);
        var pedidoItem2 = new PedidoItem(produtoId, "Pedido Item Teste", 2, 100);

        //Act
        pedido.AdicionarItemPedido(pedidoItem2);

        //Assert
        Assert.Equal(400, pedido.ValorTotal);
        Assert.Equal(1, pedido.PedidoItems.Count);
        Assert.Equal(4, pedido.PedidoItems.FirstOrDefault(p => p.Id == produtoId).Quantidade);
    }
}
