using Confere.Processos.Database;
using Confere.Processos.Modelo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Confere.Processos.Service
{
    /*
     * Maiores informações: http://legis.senado.leg.br/dadosabertos/docs/resource_MateriaService.html
     * Exemplo de matéria: http://legis.senado.leg.br/dadosabertos/materia/pls/85/2017
     */
    public class MateriasService : IDisposable
    {
        private ProcessoContext contexto;

        public MateriasService()
        {
            this.contexto = new ProcessoContext();
        }
        
        public IEnumerable<Processo> ProcessosMonitorados
        {
            get
            {
                return contexto.Processos.AsEnumerable();
            }
        }

        public async Task<MovimentacaoMateria> TramitacoesDaMateria(int codigoMateria)
        {
            string urlTramitacoes = "http://legis.senado.leg.br/dadosabertos/materia/movimentacoes/" + codigoMateria;
            Debug.WriteLine($"URL de tramitações: {urlTramitacoes}");

            using (var client = new HttpClient())
            {
                var movimentacaoResponse = await client.GetAsync(urlTramitacoes);
                if (movimentacaoResponse.IsSuccessStatusCode)
                {
                    var movimentacaoConteudo = movimentacaoResponse.Content.ReadAsStreamAsync().Result;
                    XmlSerializer movimentacaoSerializer = new XmlSerializer(typeof(MovimentacaoMateria));
                    MovimentacaoMateria movimentacao = (MovimentacaoMateria)movimentacaoSerializer.Deserialize(movimentacaoConteudo);
                    Debug.WriteLine($"Total de tramitações: {movimentacao.Materia.Tramitacoes.Length}");

                    return movimentacao;
                }
                throw new SystemException("Não foi possível consultar o serviço de tramitações.");
            }
        }

        public async Task<Materia> RecuperarMateriaPeloCodigo(Processo campos)
        {
            string urlProcesso = new UrlProcessoBuilder(campos).Build();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = await client.GetAsync(urlProcesso);
                if (response.IsSuccessStatusCode)
                {
                    var conteudo = response.Content.ReadAsStreamAsync().Result;
                    XmlSerializer serializer = new XmlSerializer(typeof(PesquisaBasicaMateria));
                    PesquisaBasicaMateria pesquisa = (PesquisaBasicaMateria)serializer.Deserialize(conteudo);

                    MovimentacaoMateria movimentacao = await TramitacoesDaMateria(pesquisa.Materia.Identificacao.Codigo);
                    pesquisa.Materia.Tramitacoes = movimentacao.Materia.Tramitacoes;
                    pesquisa.Materia.Tramitacoes.OrderBy(t => t.Identificacao.Data);
                    pesquisa.Materia.SituacaoAtual = movimentacao.Materia.SituacaoAtual;

                    return pesquisa.Materia;
                }
                throw new SystemException("Não foi possível consultar o serviço de processos.");
            }
        }

        public void Dispose()
        {
            contexto.Dispose();
        }
    }
}
