using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;

namespace Clard_Monitoramento.Vendas
{
    class VendaControl
    {
        public List<Venda> _vendas { get; set; }
        
        public VendaControl()
        {
            _vendas = new List<Venda>();
        }

        public void gerarLista()
        {
            _vendas.Clear();
            var client = new RestClient("http://clard.ddns.net:9001/api/venda/listTotal");
            var request = new RestRequest();
            var result = client.GetAsync(request).Result;
            Console.WriteLine(result.Content);

            _vendas = JsonConvert.DeserializeObject<List<Venda>>(result.Content);
        }
    }
}
