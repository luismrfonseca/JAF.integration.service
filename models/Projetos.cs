using System;
using System.Collections.Generic;
using System.Text;

namespace JAF.integration.service.models
{
    public class Projetos
    {
        public string id { get; set; }
        public string descricao { get; set; }
        public string descricao2 { get; set; }
        public string datacriacao { get; set; }
        public string dataalteracao { get; set; }
        public string datainicio { get; set; }
        public string datafim { get; set; }
        public string estado { get; set; }
        public string responsavelObra { get; set; }
        public string clienteNumero { get; set; }
        public string clienteNome { get; set; }
        public string clienteMorada1 { get; set; }
        public string clienteMorada2 { get; set; }
        public string clienteCidade { get; set; }
        public string clientePais { get; set; }
        public string clienteCodigoPostal { get; set; }
        public string clienteRegiaoCod { get; set; }
        public string clienteContato { get; set; }
        public string completa { get; set; }
        public string valorOrcamentoBase { get; set; }
        public string obraMorada1 { get; set; }
        public string obraMorada2 { get; set; }
        public string obraCidade { get; set; }
        public string obraCodigoPostal { get; set; }
        public string obraDistrito { get; set; }
        public string dataEstado { get; set; }
        public string dataEntrega { get; set; }
    }
}
