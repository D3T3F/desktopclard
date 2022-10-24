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
            _produtos = new List<Produto>();
        }

        public void cadastrar(string nome_produto, double valor, string descricao, int estoque, string categoria, string path)
        {
            var client1 = new RestClient(api_ip + "create");
            var request1 = new RestRequest();

            var body = new Produto
            {
                nome_produto = nome_produto
                                        ,
                descricao = descricao
                                        ,
                estoque = estoque
                                        ,
                valor = valor           
                                        ,
                categoria = categoria
            };


            request1.AddJsonBody(body);

            var response1 = client1.PostAsync(request1).Result;
            Console.WriteLine(response1.StatusCode.ToString() + " " + response1.Content.ToString());

            gerarLista();

            var idproduto = 0;

            foreach (Produto p in _produtos) { 
            
                if(p.id > idproduto)
                {
                    idproduto = p.id;
                }

            }

            Console.WriteLine(idproduto);

            var client = new RestClient(api_ip + "insertImg/" + idproduto);
            var request = new RestRequest();
            request.AddFile("image", path);


            var response = client.PostAsync(request).Result;
            Console.WriteLine(response.StatusCode.ToString() + " " + response.Content.ToString());
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

            var response = client.PostAsync(request).Result;
            Console.WriteLine(response.StatusCode.ToString() + " " + response.Content.ToString());

        }

        public void alterar(int alt_id, string nome_prod, string alt_descricao, int alt_quantidade, double alt_valor, string path)
        {
            var client = new RestClient(api_ip + "altProd");
            var request = new RestRequest();

            var body = new altprod
            {
                    alt_id = alt_id
             ,      alt_nome_prod = nome_prod
             ,      alt_descricao = alt_descricao
             ,      alt_quantidade = alt_quantidade
             ,      alt_valor = alt_valor
            };


            request.AddJsonBody(body);

            var response = client.PostAsync(request).Result;
            Console.WriteLine(response.StatusCode.ToString() + " " + response.Content.ToString());

            var client1 = new RestClient(api_ip + "insertImg/" + alt_id);
            var request1 = new RestRequest();
            request.AddFile("image", path);


            var response1 = client1.PostAsync(request1).Result;
            Console.WriteLine(response1.StatusCode.ToString() + " " + response1.Content.ToString());

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

            var response = client.PostAsync(request).Result;
            Console.WriteLine(response.StatusCode.ToString() + " " + response.Content.ToString());

        }
    }
}
