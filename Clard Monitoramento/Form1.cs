using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Clard_Monitoramento.Vendas;

namespace Clard_Monitoramento
{
    public partial class Form1 : Form
    {
        private ProdutoControl prodCont;
        private VendaControl vendaCont;
        private DataTable tblProd;
        private DataTable tblVenda;
        private int id_produto_alt = 0;
        private string path = "";
        private string pathAlt = "";
        public string categoria = "";

        public Form1()
        {
            InitializeComponent();
            prodCont = new ProdutoControl();
            vendaCont = new VendaControl();

            tblProd = new DataTable();
            tblVenda = new DataTable();

            tblProd.Columns.Add("Nome", typeof(string));
            tblProd.Columns.Add("Valor", typeof(string));
            tblProd.Columns.Add("Descrição", typeof(string));
            tblProd.Columns.Add("Quantidade", typeof(int));
            tblProd.Columns.Add("Status", typeof(string));
            tblProd.Columns.Add("Categoria", typeof(string));

            tblVenda.Columns.Add("Id Usuário", typeof(int));
            tblVenda.Columns.Add("Id Endereço", typeof(int));
            tblVenda.Columns.Add("Forma Pagamento",typeof(string));
            tblVenda.Columns.Add("Valor",typeof(string));
            tblVenda.Columns.Add("Status",typeof(string));
        }

        private void transformaCategoria(string nomeCombo)
        {
            if (nomeCombo == "Calças")
            {
                categoria = "calcas";
            }
            else if (nomeCombo.Trim() == "")
            {
                categoria = "";
            }
            else
            {
                categoria = nomeCombo.ToLower();
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab.Text == "VENDAS")
            {
                title.Text = "Tabela de Vendas";
            }
            else if (tabControl.SelectedTab.Text == "PRODUTOS CADASTRADOS")
            {
                title.Text = "Tabela de Produtos";
            }
            else if (tabControl.SelectedTab.Text == "CADASTRAR PRODUTO")
            {
                title.Text = "Cadastro de Produtos";
            }
            else if (tabControl.SelectedTab.Text == "ALTERAR PRODUTO")
            {
                title.Text = "Alteração de Produtos";
            }
        }

        //Configuração produtos
        private void atlTblProd()
        {   
            List<Produto> listaProd;
            DataRow Linha;

            prodCont.gerarLista();

            tblProd.Clear();
            tblProd.Rows.Clear();
            comboBoxAlt.Items.Clear();

            listaProd = prodCont._produtos;

            foreach (Produto prod in listaProd)
            {
                Linha = tblProd.NewRow();

                Linha[0] = prod.nome_produto;
                Linha[1] = prod.valor.ToString("N2");
                Linha[2] = prod.descricao;
                Linha[3] = prod.estoque;
                Linha[4] = prod.status;
                Linha[5] = prod.categoria;

                tblProd.Rows.Add(Linha);

                tabelaProd.DataSource = tblProd;

                comboBoxAlt.Items.Add(Linha[0]);
            }
        }

        private void tabelaProd_DoubleClick(object sender, EventArgs e)
        {
            tabControl.SelectTab(paginaAlterar);
            try
            {
                comboBoxAlt.Text = tabelaProd.SelectedCells[0].Value.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        private void fecharTab2_Click(object sender, EventArgs e)
        {
            tblProd.Clear();
            tblProd.Rows.Clear();
            comboBoxAlt.Items.Clear();
            prodCont._produtos.Clear();
        }

        //Criar produtos
        private void pegaImagem()
        {
             OpenFileDialog arquivo = new OpenFileDialog();
             arquivo.Title = "Open Image";
             arquivo.Filter = "png files (*.png)|*.png";


             if (arquivo.ShowDialog() == DialogResult.OK)
             {
                 try
                 {
                     Image i = new Bitmap(arquivo.FileName);
                     path = Path.GetFullPath(arquivo.FileName);

                     double imgAltura = i.PhysicalDimension.Width;
                     double imgLargura = i.PhysicalDimension.Height;

                     if (imgAltura > 1200 || imgLargura > 1200 || imgAltura < 500 || imgLargura < 500)
                     {
                        Exception exTam = new Exception("O tamanho da imagem deve estar entre 1200x1200 e 500x500");
                        throw exTam;
                     }
                     if (imgAltura != imgLargura)
                     {
                        Exception exDif = new Exception("Os dois lados da imagem devem ser iguais.");
                        throw exDif;
                     }

                     pictureBox.Image = i;
                     btnAddImg.Visible = false;
                     btnRemoveImg.Visible = true;

                 }
                 catch (Exception erro)
                 {
                     MessageBox.Show("Não foi possível carregar a imagem: \n" + erro.Message, "Erro!");
                 }

             }
             arquivo.Dispose();
        }

        private void limpaImagem()
        {
            pictureBox.Image = null;

            btnAddImg.Visible = true;
            btnRemoveImg.Visible = false;
        }

        private void limpaFormulario()
        {
            txtBoxNome.Text = "";
            txtBoxValor.Text = "";
            txtBoxQuant.Text = "";
            txtBoxDesc.Text = "";

            limpaImagem();
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {
            pegaImagem();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            pegaImagem();
        }

        private void btnRemoveImg_Click(object sender, EventArgs e)
        {
            limpaImagem();

            MessageBox.Show("Imagem Removida.");
        }

        private void btnLimpa_Click(object sender, EventArgs e)
        {
            limpaFormulario();

            MessageBox.Show("Cadastro limpo.", "Sucesso!");
        }

        private void txtBoxValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            //permite digitar apenas numeros e virgulas
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            //permite apenas uma virgula
            if ((e.KeyChar == ',') && txtBoxValor.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void txtBoxQuant_KeyPress(object sender, KeyPressEventArgs e)
        {
            //permite digitar apenas numeros
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnConfirma_Click(object sender, EventArgs e)
        {
            if (txtBoxValor.Text == "" || txtBoxDesc.Text == "" ||
                txtBoxNome.Text == "" || txtBoxQuant.Text == "" || categoria == "")
            {
                MessageBox.Show("Complete todos os campos antes de fazer o cadastro.", "Erro!");
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                var resultado = MessageBox.Show("Tem certeza que deseja concluir o cadastro?", "Aviso!", buttons);

                if (resultado == DialogResult.No)
                {
                    MessageBox.Show("Cadastro cancelado.");
                }
                else if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        prodCont.cadastrar(txtBoxNome.Text, Double.Parse(txtBoxValor.Text), txtBoxDesc.Text, int.Parse(txtBoxQuant.Text), categoria, path);
                        MessageBox.Show("Produto cadastrado com sucesso!", "Parabéns");

                        limpaFormulario();
                        atlTblProd();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }
            }
        }

        private void gerarProdutos_Click(object sender, EventArgs e)
        {
            atlTblProd();
        }

        private void txtBoxValor_Leave(object sender, EventArgs e)
        {
            if (txtBoxValor.Text.Equals("") == false)
            {
                double num = Convert.ToSingle(txtBoxValor.Text);
                txtBoxValor.Text = num.ToString("N2");
            }
        }

        private void comboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            transformaCategoria(comboCategoria.Text);   
        }

        //Atualizar produtos
        private void limpaFormularioAtl()
        {
            txtNomeAlt.Text = "";
            txtValorAlt.Text = "";
            txtQuantAlt.Text = "";
            txtDescAlt.Text = "";
            
            comboBoxAlt.SelectedItem = null;
            comboBoxAlt.Text = "";

            comboCategoriaAlt.SelectedItem = null;
            comboCategoriaAlt.Text = "";
        }

        private void comboBoxAlt_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Produto> listaProd = prodCont._produtos;

            foreach (Produto prod in listaProd)
            {
                if (prod.nome_produto == comboBoxAlt.Text)
                {
                    txtNomeAlt.Text = prod.nome_produto;
                    txtValorAlt.Text = prod.valor.ToString("N2");
                    txtDescAlt.Text = prod.descricao;
                    txtQuantAlt.Text = prod.estoque.ToString();
                    id_produto_alt = prod.id;
                    comboCategoriaAlt.Text = prod.categoria;

                    if (prod.status == "ativo")
                    {
                        btnAtrExc.Text = "Excluir";
                    }
                    else if (prod.status == "excluido")
                    {
                        btnAtrExc.Text = "Ativar";
                    }
                }
            }
        }

        private void btnAlt_Click(object sender, EventArgs e)
        {
            if (txtValorAlt.Text == "" || txtDescAlt.Text == "" ||
                txtNomeAlt.Text == "" || txtQuantAlt.Text == "")
            {
                MessageBox.Show("Não deixe nenhum campo vazio para fazer a alteração.", "Erro!");
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                var resultado = MessageBox.Show("Tem certeza que deseja realizar a alteração?", "Aviso!", buttons);

                if (resultado == DialogResult.No)
                {
                    MessageBox.Show("Alteração cancelada.");
                }
                else if (resultado == DialogResult.Yes)
                {
                    try
                    {
                        List<Produto> listaProd = prodCont._produtos;

                        foreach (Produto p in listaProd)
                        {
                            if (p.nome_produto.Equals(comboBoxAlt.Text))
                            {
                                prodCont.alterar(id_produto_alt, txtNomeAlt.Text, txtDescAlt.Text, int.Parse(txtQuantAlt.Text), Double.Parse(txtValorAlt.Text), categoria,pathAlt);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Houve algum problema na alteração do produto: " + ex.Message);
                    }
                    finally
                    {
                        MessageBox.Show("Produto alterado com sucesso!");

                        atlTblProd();

                        limpaFormularioAtl();
                    }
                }
            }
        }

        private void txtValorAlt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && txtValorAlt.Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void txtValorAlt_Leave(object sender, EventArgs e)
        {
            if (txtValorAlt.Text.Equals("") == false)
            {
                double num = Convert.ToSingle(txtValorAlt.Text);
                txtValorAlt.Text = num.ToString("N2");
            }
        }

        private void txtQuantAlt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboCategoriaAlt_SelectedIndexChanged(object sender, EventArgs e)
        {
            transformaCategoria(comboCategoriaAlt.Text);
        }

        private void btnAtrExc_Click(object sender, EventArgs e)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            var resultado = MessageBox.Show("Tem certeza que deseja " + btnAtrExc.Text.ToLower() + " esse produto?", "Aviso!", buttons);

            if (resultado == DialogResult.No)
            {
                MessageBox.Show("Mudança de status cancelada.");
            }
            else if (resultado == DialogResult.Yes)
            {
                try
                {
                    List<Produto> produtos = prodCont._produtos;

                    foreach (Produto p in produtos)
                    {
                        if (p.nome_produto.Equals(comboBoxAlt.Text))
                        {
                            if (p.status == "ativo")
                                prodCont.remover(id_produto_alt);

                            else if (p.status == "excluido")
                                prodCont.ativar(id_produto_alt);
                        }
                    }

                    limpaFormularioAtl();
                    btnAtrExc.Text = "Excluir/Altivar";
                }          
                catch (Exception ex)
                {
                    MessageBox.Show("Houve algum problema na exclusão/ativação do produto: " + ex.Message);
                }
                finally
                {
                    atlTblProd();
                }
            }
        }

        //Configuração vendas
        private void atlTblVendas()
        {
            List<Venda> listVendas;
            DataRow Linha;

            vendaCont.gerarLista();

            tblVenda.Clear();
            tblVenda.Rows.Clear();
         

            listVendas = vendaCont._vendas;

            foreach (Venda vend in listVendas)
            {
                Linha = tblVenda.NewRow();

                Linha[0] = vend.id_usuario;
                Linha[1] = vend.id_endereco;
                Linha[2] = vend.forma_pgto;
                Linha[3] = vend.valor_total.ToString("N2");
                Linha[4] = vend.status_entrega;

                tblVenda.Rows.Add(Linha);

                tabelaVenda.DataSource = tblVenda;
            }
        }

        private void gerarVendas_Click(object sender, EventArgs e)
        {
            atlTblVendas();
        }       

        private void fecharTab1_Click(object sender, EventArgs e)
        {
            tblVenda.Clear();
            tblVenda.Rows.Clear();
            vendaCont._vendas.Clear();
        }
    }
}
