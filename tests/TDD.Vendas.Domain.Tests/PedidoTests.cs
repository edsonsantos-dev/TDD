﻿namespace TDD.Vendas.Domain.Tests;

public class PedidoTests
{
    [Fact(DisplayName = "Adicionar Item Novo Pedido")]
    [Trait("Categoria", "Pedido Tests")]
   public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
    {
        // Arrange
        var pedido = new Pedido();
        var pedidoItem = new PedidoItem(Guid.NewGuid(), "Pedido Item Teste", 2, 100);


        //Act
        pedido.AdicionarItemPedido(pedidoItem);


        //Assert
        Assert.Equal(200, pedido.ValorTotal);
    }
}
