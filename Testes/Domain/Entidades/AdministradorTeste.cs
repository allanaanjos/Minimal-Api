
using MinimalApi.Domain.Entities;

namespace Testes.Domain.Entidades
{

    [TestClass]
    public class AdministradorTeste
    {
        [TestMethod]
        public void TestandoGetSetAdministrador()
        {
            var adm = new Administrador();

            adm.Id = 1;
            adm.Email = "joao@gmail.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("joao@gmail.com", adm.Email);
            Assert.AreEqual("teste", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);

            
        }

    }
}