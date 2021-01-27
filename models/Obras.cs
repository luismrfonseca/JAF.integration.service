using System;
using System.Collections.Generic;
using System.Text;

namespace JAF.integration.service.models
{
    public class Obras
    {
        public string id { get; set; }
        public string descricao { get; set; }
        public string descricao2 { get; set; }
        public DateTime datacriacao { get; set; }
        public DateTime dataalteracao { get; set; }
        public DateTime datainicio { get; set; }
        public DateTime datafim { get; set; }
        public int estado { get; set; }
        public string responsavelObra { get; set; }
        public string clienteNumero { get; set; }
        public string clienteNome { get; set; }
        public string clienteMorada { get; set; }
        public string clienteMorada2 { get; set; }
        public string clienteCidade { get; set; }
        public string clientePais { get; set; }
        public string clienteCodigoPostal { get; set; }
        public string clienteRegiaoCod { get; set; }
        public string clienteContato { get; set; }
        public int completa { get; set; }
        public Decimal valorOrcamentoBase { get; set; }
        public string obraMorada1 { get; set; }
        public string obraMorada2 { get; set; }
        public string obraCidade { get; set; }
        public string obraCodigoPostal { get; set; }
        public string obraDistrito { get; set; }
        public DateTime dataEstado { get; set; }
        public DateTime dataEntrega { get; set; }
    }
}
