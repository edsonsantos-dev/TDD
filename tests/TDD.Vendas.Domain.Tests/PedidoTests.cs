using TDD.Core.DomainObjects;

namespace TDD.Vendas.Domain.Tests;

public class PedidoTests
{
    [Fact(DisplayName = "Adicionar Item Novo Pedido")]
    [Trait("Categoria", "Vendas - Pedido")]
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
    [Trait("Categoria", "Vendas - Pedido")]
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



    [Fact(DisplayName = "Adicionar Item Pedido Com Soma Unidades Acima Do Permitido")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItem = new PedidoItem(produtoId, "Pedido Item Teste", 2, 100);
        pedido.AdicionarItemPedido(pedidoItem);
        var pedidoItem2 = new PedidoItem(produtoId, "Pedido Item Teste", Pedido.MAX_UNIDADES_ITEM, 100);

        //Act & Assert
        Assert.Throws<DomainException>(() => pedido.AdicionarItemPedido(pedidoItem2));
    }

    [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var pedidoItem = new PedidoItem(Guid.NewGuid(), "Pedido Item Teste", 2, 100);

        //Act & Assert
        Assert.Throws<DomainException>(() => pedido.AtualizarItemPedido(pedidoItem));
    }

    [Fact(DisplayName = "Atualizar Item Pedido Valido")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
        pedido.AdicionarItemPedido(pedidoItem);
        var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
        var novaQuantidade = pedidoItemAtualizado.Quantidade;

        // Act
        pedido.AtualizarItemPedido(pedidoItemAtualizado);

        // Assert
        Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.Id == produtoId).Quantidade);
    }

    [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
        var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
        pedido.AdicionarItemPedido(pedidoItemExistente1);
        pedido.AdicionarItemPedido(pedidoItemExistente2);

        var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
        var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                          pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;

        // Act
        pedido.AtualizarItemPedido(pedidoItemAtualizado);

        // Assert
        Assert.Equal(totalPedido, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Remover Item Pedido Inexistente")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => pedido.RemoverItemPedido(pedidoItemRemover));
    }


    [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
        var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
        pedido.AdicionarItemPedido(pedidoItem1);
        pedido.AdicionarItemPedido(pedidoItem2);

        var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

        // Act
        pedido.RemoverItemPedido(pedidoItem1);

        // Assert
        Assert.Equal(totalPedido, pedido.ValorTotal);
    }
}