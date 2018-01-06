using Confere.Processos.Modelo;

namespace Confere.Processos.Service
{
    public class UrlProcessoBuilder
    {
        private const string URL_BASICA = "http://legis.senado.leg.br/dadosabertos/materia";
        private Processo campos;

        public UrlProcessoBuilder(Processo campos)
        {
            if (string.IsNullOrEmpty(campos.Sigla))
            {
                throw new System.ArgumentNullException($"Campo Sigla está inválido!");
            }
            if (campos.Numero<=0)
            {
                throw new System.ArgumentNullException($"Campo Numero está inválido!");
            }
            if (campos.Ano<=0)
            {
                throw new System.ArgumentNullException($"Campo Ano está inválido!");
            }
            this.campos = campos;
        }

        public string UrlBasica
        {
            get { return URL_BASICA; }
        }

        public string Build()
        {
            return $"{URL_BASICA}/{campos.Sigla}/{campos.Numero}/{campos.Ano}";
        }
    }
}