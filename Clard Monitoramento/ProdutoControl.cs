using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Clard_Monitoramento
{
    internal class ProdutoControl
    {
        private string api_ip = "http://clard.ddns.net:9001/api/product/";
        public List<Produto> _produtos { get; set; }

        public ProdutoControl()
        {
            //Instanciação da lista de produtos
            _produtos = new List<Produto>();
        }

        //Função de POST na API 
        private void PostLista(RestClient client, RestRequest request)
        {
            var response = client.PostAsync(request).Result;
            Console.WriteLine(response.StatusCode.ToString() + " " + response.Content.ToString());
        }

        public void cadastrar(string nome_produto, double valor, string descricao, int estoque, string categoria, string path)
        {
            var client1 = new RestClient(api_ip + "create");
            var request1 = new RestRequest();

            var body = new Produto
            {
                nome_produto = nome_produto,
                descricao = descricao,
                estoque = estoque,
                valor = valor,
                categoria = categoria
            };

            request1.AddJsonBody(body);

            PostLista(client1, request1);

            gerarLista();

            //Verifica id do ultimo produto existente para fazer o POST da imagem com seu id
            var idproduto = 0;
            foreach (Produto p in _produtos) {        
                if(p.id > idproduto)
                {
                    idproduto = p.id;
                }
            }

            var client = new RestClient(api_ip + "insertImg/" + idproduto);
            var request = new RestRequest();
            request.AddFile("image", path);

            PostLista(client, request);
        }

        public void gerarLista()
        {
            _produtos.Clear();
            var client = new RestClient(api_ip + "list");
            var request = new RestRequest();
            var result = client.GetAsync(request).Result;
            Console.WriteLine(result.Content); 

            _produtos = JsonConvert.DeserializeObject<List<Produto>>(result.Content);
        }

        public void remover(int id)
        {
            var client = new RestClient(api_ip + "deleteProd");
            var request = new RestRequest();

            var body = new exprod
            {
                idprodex = id
            };

            request.AddJsonBody(body);

            PostLista(client, request);
        }

        public void alterar(int alt_id, string nome_prod, string alt_descricao, int alt_quantidade, double alt_valor, string alt_categoria,string path)
        {
            var client = new RestClient(api_ip + "altProd");
            var request = new RestRequest();

            var body = new altprod
            {
                alt_id = alt_id,
                alt_nome_prod = nome_prod,
                alt_descricao = alt_descricao,
                alt_quantidade = alt_quantidade,
                alt_valor = alt_valor,
                alt_categoria = alt_categoria
            };

            request.AddJsonBody(body);

            PostLista(client, request);
        }

        public void ativar(int id)
        {
            var client = new RestClient(api_ip + "ativarProd");
            var request = new RestRequest();

            var body = new exprod
            {
                idprodex = id
            };

            request.AddJsonBody(body);

            PostLista(client, request);
        }
    }
}
