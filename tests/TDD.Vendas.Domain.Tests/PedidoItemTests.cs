using TDD.Core.DomainObjects;

namespace TDD.Vendas.Domain.Tests;

public class PedidoItemTests
{
    [Fact(DisplayName = "Novo Item Pedido Com Unidades Acima Do Permitido")]
    [Trait("Categoria", "Vendas - Item Pedido")]
    public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
    {
        //Arrange & Act & Assert
        Assert.Throws<DomainException>(() =>
            new PedidoItem(Guid.NewGuid(), "Pedido Item Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100));
    }

    [Fact(DisplayName = "Novo Item Pedido Com Unidades Abaixo Do Permitido")]
    [Trait("Categoria", "Vendas - Item Pedido")]
    public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
    {
        //Arrange & Act & Assert
        Assert.Throws<DomainException>(() =>
            new PedidoItem(Guid.NewGuid(), "Pedido Item Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
    }
}
