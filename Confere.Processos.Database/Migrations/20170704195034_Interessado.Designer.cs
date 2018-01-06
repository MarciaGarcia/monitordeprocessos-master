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
    [Migration("20170704195034_Interessado")]
    partial class Interessado
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Confere.Processos.Modelo.Interessado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Interessados");
                });

            modelBuilder.Entity("Confere.Processos.Modelo.Interesse", b =>
                {
                    b.Property<int>("InteressadoId");

                    b.Property<int>("ProcessoId");

                    b.HasKey("InteressadoId", "ProcessoId");

                    b.HasIndex("ProcessoId");

                    b.ToTable("Interesse");
                });

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

            modelBuilder.Entity("Confere.Processos.Modelo.Interesse", b =>
                {
                    b.HasOne("Confere.Processos.Modelo.Interessado", "Interessado")
                        .WithMany("Interesses")
                        .HasForeignKey("InteressadoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Confere.Processos.Modelo.Processo", "Processo")
                        .WithMany("Interesses")
                        .HasForeignKey("ProcessoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
