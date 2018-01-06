using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Confere.Processos.Database;
using Confere.Processos.Modelo;

namespace Confere.Processos.Database.Migrations
{
    [DbContext(typeof(ProcessoContext))]
    [Migration("20170703210055_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Confere.Processos.Modelo.Processo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Ano");

                    b.Property<int>("Codigo");

                    b.Property<DateTime?>("DataUltimaAtualizacao");

                    b.Property<string>("Emenda");

                    b.Property<int>("Numero");

                    b.Property<int>("Origem");

                    b.Property<string>("Sigla")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Processos");
                });
        }
    }
}
