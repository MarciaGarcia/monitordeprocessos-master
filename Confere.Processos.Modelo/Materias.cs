using System;
using System.Xml.Serialization;

namespace Confere.Processos.Modelo
{
    [Serializable()]
    public class IdentificacaoTramitacao
    {
        [XmlElement("TextoTramitacao")]
        public string Texto { get; set; }

        [XmlElement("DataTramitacao", DataType = "date")]
        public DateTime Data { get; set; }
    }

    [Serializable()]
    public class Tramitacao
    {
        [XmlElement("IdentificacaoTramitacao")]
        public IdentificacaoTramitacao Identificacao { get; set; }
    }

    [Serializable()]
    public class IdentificacaoMateria
    {
        [XmlElement("CodigoMateria")]
        public int Codigo { get; set; }

        [XmlElement("SiglaCasaIdentificacaoMateria")]
        public string SiglaCasa { get; set; }

        [XmlElement("NumeroMateria")]
        public string Numero { get; set; }

        [XmlElement("AnoMateria")]
        public string Ano { get; set; }

        [XmlElement("SiglaSubtipoMateria")]
        public string SiglaSubtipoMateria { get; set; }
    }

    [Serializable()]
    public class DadosBasicosMateria
    {
        [XmlElement("EmentaMateria")]
        public string Ementa { get; set; }

        [XmlElement("ObservacaoMateria")]
        public string Observacao { get; set; }

        [XmlElement("IndexacaoMateria")]
        public string Indexacao { get; set; }


        public string IndicadorComplementar { get; set; }
        public DateTime DataApresentacao { get; set; }
    }

    [Serializable()]
    public class SituacaoAutuacao
    {
        [XmlElement(DataType = "date")]
        public DateTime Data { get; set; }

        [XmlElement("CodigoSituacao")]
        public int Codigo { get; set; }

        [XmlElement("SiglaSituacao")]
        public string Sigla { get; set; }

        [XmlElement("DescricaoSituacao")]
        public string Descricao { get; set; }
    }

    [Serializable()]
    public class Autuacao
    {
        [XmlElement("NumeroAutuacao")]
        public int NumeroAutuacao { get; set; }

        [XmlElement("Situacao")]
        public SituacaoAutuacao Situacao { get; set; }
    }

    [Serializable()]
    public class SituacaoAtual
    {
        [XmlArray("Autuacoes")]
        [XmlArrayItem("Autuacao")]
        public Autuacao[] Autuacoes { get; set; }
    }

    [Serializable()]
    public class Materia
    {
        [XmlElement("IdentificacaoMateria")]
        public IdentificacaoMateria Identificacao { get; set; }

        [XmlElement("DadosBasicosMateria")]
        public DadosBasicosMateria DadosBasicos { get; set; }

        [XmlArray("Autoria")]
        [XmlArrayItem("Autor")]
        public Autor[] Autoria { get; set; }

        [XmlArray("Tramitacoes")]
        [XmlArrayItem("Tramitacao")]
        public Tramitacao[] Tramitacoes { get; set; }

        //public object AutoresPrincipais { get; set; }

        [XmlElement("SituacaoAtual")]
        public SituacaoAtual SituacaoAtual { get; set; }

        public override string ToString()
        {
            return $"{Identificacao.SiglaSubtipoMateria}/{Identificacao.Numero}/{Identificacao.Ano}";
        }

    }

    [Serializable()]
    public class IdentificacaoParlamentar
    {
        [XmlElement("NomeParlamentar")]
        public string NomeParlamentar { get; set; }

        [XmlElement("NomeCompletoParlamentar")]
        public string NomeCompletoParlamentar { get; set; }

        [XmlElement("FormaTratamento")]
        public string FormaTratamento { get; set; }

        [XmlElement("UrlFotoParlamentar")]
        public string UrlFotoParlamentar { get; set; }

        [XmlElement("UrlPaginaParlamentar")]
        public string UrlPaginaParlamentar { get; set; }

        [XmlElement("EmailParlamentar")]
        public string EmailParlamentar { get; set; }

        [XmlElement("SiglaPartidoParlamentar")]
        public string SiglaPartidoParlamentar { get; set; }

        [XmlElement("UfParlamentar")]
        public string UfParlamentar { get; set; }
    }

    [Serializable()]
    public class Autor
    {
        [XmlElement("NomeAutor")]
        public string NomeAutor { get; set; }

        [XmlElement("SiglaTipoAutor")]
        public string SiglaTipoAutor { get; set; }

        [XmlElement("DescricaoTipoAutor")]
        public string DescricaoTipoAutor { get; set; }

        [XmlElement("UfAutor")]
        public string UfAutor { get; set; }

        [XmlElement("IdentificacaoParlamentar")]
        public IdentificacaoParlamentar Identificacao { get; set; }
    }

    [Serializable()]
    [XmlRoot("DetalheMateria")]
    public class PesquisaBasicaMateria
    {
        [XmlElement("Materia", typeof(Materia))]
        public Materia Materia { get; set; }
    }

    [Serializable()]
    [XmlRoot("MovimentacaoMateria")]
    public class MovimentacaoMateria
    {
        [XmlElement("Materia", typeof(Materia))]
        public Materia Materia { get; set; }
    }

    
}
