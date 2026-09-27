using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Application.Products;
using CosmeticsShop.Domain.Entities;
using Moq;
using Xunit;

namespace CosmeticsShop.Tests.Application;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateProductAsync_WithValidData_CallsRepositoryAddAndSave()
    {
        // ============================================================
        // ARRANGE — on prépare le terrain
        // ============================================================

        // Mock<IProductRepository> crée un FAUX repository — un objet qui
        // "prétend" être un IProductRepository, mais sans aucun vrai code
        // derrière. Aucune connexion à SQL Server, rien de réel ne se passe.
        // C'est comme un "acteur" qui joue le rôle du repository, juste pour
        // ce test précis.
        var mockRepository = new Mock<IProductRepository>();

        // ProductService a besoin d'un IProductRepository dans son constructeur.
        // On lui donne "mockRepository.Object" — c'est la version UTILISABLE
        // du mock, celle qu'on peut réellement passer en paramètre, comme si
        // c'était un vrai repository.
        var service = new ProductService(mockRepository.Object);

        var categoryId = Guid.NewGuid();

        // ============================================================
        // ACT — on exécute l'action qu'on veut tester
        // ============================================================

        // On appelle la VRAIE méthode CreateProductAsync de ProductService.
        // À l'intérieur, ProductService va appeler _productRepository.AddAsync(...)
        // et _productRepository.SaveChangesAsync(...) — mais comme _productRepository
        // EST le mock, ces appels ne touchent JAMAIS une vraie base de données.
        // Le mock se contente d'ENREGISTRER, en mémoire, que ces méthodes
        // ont été appelées (et avec quels paramètres).
        var result = await service.CreateProductAsync(
            "Baume à lèvres karité", "Description", 6.50m, 40, categoryId);

        // ============================================================
        // ASSERT — on vérifie que tout s'est bien passé
        // ============================================================

        // Vérification n°1 : le DTO retourné contient bien les bonnes valeurs.
        // Ça prouve que Product a été créé correctement (règles du Domain
        // respectées) et que MapToDto a bien fonctionné.
        Assert.Equal("Baume à lèvres karité", result.Name);
        Assert.Equal(6.50m, result.Price);

        // Vérification n°2 : on interroge le MOCK lui-même — pas le résultat,
        // mais ce qui s'est PASSÉ à l'intérieur de ProductService.
        // "Verify" pose la question : "AddAsync a-t-elle été appelée,
        // avec N'IMPORTE QUEL Product (It.IsAny<Product>()), EXACTEMENT
        // 1 FOIS (Times.Once) ?"
        //
        // Si ProductService avait un bug et OUBLIAIT d'appeler AddAsync,
        // cette ligne ferait ÉCHOUER le test ici, avec un message d'erreur
        // clair — sans jamais avoir eu besoin d'une vraie base de données
        // pour détecter ce bug.
        mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>(), default), Times.Once);

        // Vérification n°3 : même principe, mais pour SaveChangesAsync —
        // on s'assure que la "vraie sauvegarde" a bien été demandée aussi
        // (rappelle-toi : AddAsync seul ne suffit pas, il faut aussi
        // SaveChangesAsync pour que ce soit "confirmé").
        mockRepository.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}