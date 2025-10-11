# 💰 Midas — Aplicativo de Controle Financeiro

O Midas é um app mobile que ajuda usuários a gerenciar suas finanças pessoais, controlar gastos recorrentes, definir metas e prever o saldo futuro com base no histórico de transações.

## 🚀 Funcionalidades
- Registro de ganhos e gastos
- Controle de assinaturas e contas recorrentes
- Metas financeiras com progresso visual
- Previsão de saldo no fim do mês (baseado em histórico e gastos médios)
- Leitura de extrato bancário
- Cadastro e autenticação dos usuários

## Requisitos funcionais
- O sistema deve permitir o cadastro de usuários com autenticação segura
- O usuário deve poder registrar transações financeiras (ganhos e gastos)
- O sistema deve permitir a categorização das transações
- O usuário deve poder cadastrar despesas recorrentes
- O sistema deve calcular e exibir o saldo previsto para o fim do mês
- O usuário deve poder criar metas financeiras com valor alvo e progresso
- O sistema deve gerar relatórios mensais com resumo de gastos
- O sistema deve realizar a leitura dos arquivos de extrato bancário enviados 

## Requisitos não funcionais
- A aplicação deve ser desenvolvida com arquitetura limpa (Clean Architecture)
- A API deve ser construída em ASP.NET Core com Entity Framework
- O sistema deve utilizar banco de dados
- O app mobile deve ser desenvolvido com React Native
- O sistema deve garantir segurança no armazenamento de dados sensíveis
- O sistema deve ser intuitivo para o usuário final
- O código deve seguir boas práticas de versionamento e testes automatizados

## 🧱 Tecnologia
- .NET 9 com ASP.NET Core (API)
- Entity Framework Core
- Clean Architecture

## 📁 Estrutura do Projeto
- **Controllers**: Camada de apresentação (API endpoints)
- **DTOs**: Objetos de transferência de dados
- **UseCase**: Lógica de negócio e regras de domínio
- **Infrastructure**: Acesso a dados e persistência
- **Utils**: Utilitários e configurações
- 
- A API Midas possui os seguintes controladores:
- **📊 Categoria**: `/api/categoria`
- **👤 Usuario**: `/api/usuario`
- **🪙 Cofrinho**: `/api/cofrinho`
- **💸 Gasto**: `/api/gasto`
- **💰 Receita**: `/api/receita`

## Instruções para rodar o projeto localmente
   #### Ambiente de Desenvolvimento
- **Swagger UI**: `https://localhost:7018/swagger` ou `http://localhost:5220/swagger`
          **Nota**: Em outros casos, substitua pelas portas específicas configuradas no seu projeto. As portas padrão são exibidas no console quando a aplicação inicia.
- Verifique se o Swagger está configurado no `Program.cs`

## 👥 Equipe
- Barbara Bonome Filipus - RM 560431 | 2TDSPR
- Vinicius Lira Ruggeri - RM 560593 | 2TDSPR
- Yasmin Pereira da Silva - RM 560039 | 2TDSPR

