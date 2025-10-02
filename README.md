# 💰 Midas — Aplicativo de Controle Financeiro

O Midas é um app mobile que ajuda usuários a gerenciar suas finanças pessoais, controlar gastos recorrentes, definir metas e prever o saldo futuro com base no histórico de transações.

## 🚀 Funcionalidades
- Registro de ganhos e gastos
- Controle de assinaturas e contas recorrentes
- Metas financeiras com progresso visual
- Previsão de saldo no fim do mês (baseado em histórico e gastos médios)
- Leitura de extrato bancário
- Entrada de dados por voz
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
- O app deve permitir entrada de dados por voz

## Requisitos não funcionais
- A aplicação deve ser desenvolvida com arquitetura limpa (Clean Architecture)
- A API deve ser construída em ASP.NET Core com Entity Framework **>>>>>>>>>>>>>>>> validar**
- O sistema deve utilizar banco de dados relacional (SQL Server ou SQLite) **>>>>>>>>>>>>>>>> validar**
- O app mobile deve ser desenvolvido com React Native
- O sistema deve garantir segurança no armazenamento de dados sensíveis
- O tempo de resposta da API deve ser inferior a 500ms para operações básicas
- O sistema deve ser intuitivo para o usuário final
- O código deve seguir boas práticas de versionamento e testes automatizados

## 🧱 Tecnologias >>>>> validar
- .NET 8 com ASP.NET Core (API)
- Entity Framework Core
- Clean Architecture

## 📁 Estrutura do Projeto  >>>>> validar
- `Midas.Domain` — Entidades e interfaces
- `Midas.Application` — Regras de negócio e casos de uso
- `Midas.Infrastructure` — Persistência e serviços externos
- `Midas.API` — Camada de apresentação (Web API)
- `Midas.Tests` — Testes unitários e de integração

## 👥 Equipe
- Barbara Bonome Filipus - RM 560431 | 2TDSPR
- Vinicius Lira Ruggeri - RM 560593 | 2TDSPR
- Yasmin Pereira da Silva - RM 560039 | 2TDSPR

