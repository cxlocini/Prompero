# Sistema de Controle de Produtos

Este é um sistema CRUD completo para gerenciamento de produtos desenvolvido em C# com Entity Framework Core e PostgreSQL.

## Funcionalidades Implementadas

✅ **CRUD Completo de Produtos**
- Criar novos produtos
- Consultar produtos existentes
- Atualizar dados de produtos
- Excluir produtos

✅ **Consulta por Descrição**
- Busca em tempo real durante digitação (evento KeyUp)
- Exibe código, descrição, data validade, preço final e prazo validade em dias

✅ **Botão Listar**
- Mostra todos os produtos da tabela no DataGridView

✅ **Gráfico de Análise**
- Formulário separado com gráfico de colunas
- Descrição na base da coluna
- Duas colunas: lucro em reais e prazo validade em dias

✅ **Preenchimento Automático**
- Ao digitar código e perder foco, preenche outros campos automaticamente

✅ **Seleção por Duplo Clique**
- Duplo clique em linha do DataGridView transfere dados para os campos

## Configuração do Banco de Dados

### 1. PostgreSQL
- Instale o PostgreSQL
- Crie um banco de dados chamado `TOCC8`
- Execute o script `criar_tabela_produto.sql` para criar a tabela

### 2. String de Conexão
A string de conexão está configurada no arquivo `AppDbContext.cs`:
```
Host=localhost;Database=TOCC8;Username=postgres;Password=1234
```

**Ajuste conforme sua configuração do PostgreSQL.**

## Estrutura da Tabela

```sql
CREATE TABLE produto (
    codigo SERIAL PRIMARY KEY,
    descricao VARCHAR(100),
    datavalidade DATE,
    preco FLOAT,
    taxalucro FLOAT
);
```

## Propriedades Calculadas

O modelo `Produto` inclui propriedades calculadas:

- **PrecoFinal**: Preço + (Preço × TaxaLucro / 100)
- **PrazoValidade**: Dias restantes até a data de validade
- **LucroEmReais**: PreçoFinal - Preço

## Como Usar

1. **Compilar o Projeto**
   - Abra a solução no Visual Studio
   - Restaure os pacotes NuGet
   - Compile o projeto

2. **Executar o Sistema**
   - Execute o programa
   - O sistema criará automaticamente o banco se não existir

3. **Operações Disponíveis**
   - **Novo**: Limpa os campos para inserção
   - **Salvar**: Salva novo produto ou atualiza existente
   - **Excluir**: Remove produto selecionado
   - **Listar**: Mostra todos os produtos
   - **Gráfico**: Abre formulário com gráfico de análise

## Tecnologias Utilizadas

- **C#** - Linguagem de programação
- **Windows Forms** - Interface gráfica
- **Entity Framework Core** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados
- **System.Windows.Forms.DataVisualization** - Gráficos

## Pontuação

Este sistema utiliza **Entity Framework Core** (não perde pontos) para conexão com o banco de dados PostgreSQL.

## Dados de Exemplo

O script SQL inclui 10 produtos de exemplo com diferentes datas de validade, preços e taxas de lucro para testar todas as funcionalidades do sistema.
