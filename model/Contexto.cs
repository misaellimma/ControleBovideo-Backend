using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleBovideo.model;

namespace ControleBovideo.model
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> opcoes) : base (opcoes) { }
        public DbSet<Endereco> Enderecos{ get; set; }
        public DbSet<EspecieBovideo> EspecieBovideos{ get; set; }
        public DbSet<FinalidadeVenda> FinalidadeVendas{ get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Produtor> Produtores{ get; set; }
        public DbSet<Propriedade> Propriedades{ get; set; }
        public DbSet<Rebanho> Rebanhos{ get; set; }
        public DbSet<RegistroVacina> RegistroVacinas{ get; set; }
        public DbSet<Vacina> Vacinas{ get; set; }
        public DbSet<Venda> Vendas{ get; set; }
    }
}
