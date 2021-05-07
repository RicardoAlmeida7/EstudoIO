using ByteBankImportacaoExportacao.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBankImportacaoExportacao 
{ 
    partial class Program 
    { 
        static void Main(string[] args) 
        {

            copiaArquivos("contas.txt", "contas_copia.txt");

            Console.ReadLine();
        }
        static void CriarArquivosComSWtriter()
        {
            var caminhoArquivo = "teste.txt";
            using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream)) 
            {
                var informacaoConta = "1001, 2000-4, 4500.00, Maria";
                streamWriter.Write(informacaoConta);
            }
        }
        static ContaCorrente AdicionadorDeContas(string linha)
        {
            var campo = linha.Split(',');//divide a sttring linha a cada virgula e retorna dentro de array de string
            //guarda cada valor em um campo dentro do array
            var agencia = campo[0];
            var numero = campo[1];
            var saldo = campo[2].Replace('.',',');
            var nomeTitular = campo[3];
            //faz um parse para cada tipo
            int agenciaInt = int.Parse(agencia);
            int numeroInt = int.Parse(numero);
            double saldoDouble = double.Parse(saldo);
            //cria uma conta com os parametros ja convertidos para seus respecitvos tipo
            var contaCriada = new ContaCorrente(agenciaInt, numeroInt);
            contaCriada.Depositar(saldoDouble);
            
            var cliente = new Cliente();
            cliente.Nome = nomeTitular;
            contaCriada.Titular = cliente;
            //retorna a conta criada com o cliente associado
            return contaCriada;
        }
        static void CriarArquivos()
        {
            var caminhoArquivo = "contas_gravada.csv";
            using(var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                var informacaoContas = "1001, 987654321, 4000.10, Ricardo Almeida";
                var utfEncondering = Encoding.UTF8; //define tipo de codificacao 

                //codifica string devolvendo em um array de byte
                var arquivoCodificado = utfEncondering.GetBytes(informacaoContas);

                //cria arquivo
                fileStream.Write(arquivoCodificado, 0, arquivoCodificado.Length);
                
            }
        }
        static void LerArquivo(string arquivoOrigem)
        {
            var caminhoDoArquivo = arquivoOrigem;
            using (var fluxoDeStream = new FileStream(caminhoDoArquivo, FileMode.Open))
            using (var streamReader = new StreamReader(fluxoDeStream))
            {
                while (!streamReader.EndOfStream) //ler até o fim da stream
                {
                    var linha = streamReader.ReadLine(); // guarda a linha em uma variavel
                    var contaCorrente = AdicionadorDeContas(linha);// atribui os valores lidos por linha em cada conta
                    Console.WriteLine($"conta: {contaCorrente.Numero}, Agencia: {contaCorrente.Agencia}," +
                                        $" saldo: R${contaCorrente.Saldo}, Titular: {contaCorrente.Titular.Nome}");
                }
            }
 
        }
        static void copiaArquivos(string arquivoOrigem, string arquivoDestino)
        {
            var caminhoOrigem = arquivoOrigem;
            var caminhoDestino = arquivoDestino;
            using (var fileStreamReader = new FileStream(caminhoOrigem, FileMode.Open))
            using (var streamReader = new StreamReader(fileStreamReader))
            using (var fileStreamWriter = new FileStream(caminhoDestino, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStreamWriter))
            {
                while (!streamReader.EndOfStream)
                {
                    var linha = streamReader.ReadLine();
                    streamWriter.Flush();// despeja o buffer do StreamWriter
                    streamWriter.WriteLine(linha);
                }
            }
            
        }
    }
} 
 