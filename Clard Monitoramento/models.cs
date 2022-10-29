namespace Clard_Monitoramento
{
    class exprod
    {
        public int idprodex { get; set; }
    }

    class altprod
    {
        public int     alt_id { get; set; }
        public string  alt_nome_prod { get; set; }
        public string  alt_descricao { get; set; }
        public int     alt_quantidade { get; set; }
        public double  alt_valor { get; set; }
        public string  alt_categoria { get; set; }
    }

    class Produto
    {
        public int id { get; set; }
        public string nome_produto { get; set; }
        public string descricao { get; set; }
        public double valor { get; set; }
        public int estoque { get; set; }
        public string status { get; set; }
        public string categoria { get; set; }
    }

    class Venda
    {
        public int id_usuario { get; set; }
        public int id_endereco { get; set; }
        public string forma_pgto { get; set; }
        public double valor_total { get; set; }
        public string status_entrega { get; set; }
    }
}
