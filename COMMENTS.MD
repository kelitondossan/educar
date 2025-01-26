# COMMENTS.md

## Decisão da Arquitetura Utilizada

O projeto foi dividido em duas camadas principais:

1. **Backend**:
   - Criado em .NET Core com padrão de arquitetura limpa.
   - Utiliza **Entity Framework Core** para o gerenciamento do banco de dados relacional.
   - A API segue o padrão RESTful para comunicação com o frontend.
   - Repositórios e serviços foram utilizados para organizar a lógica de negócios e a persistência.

2. **Frontend**:
   - Criado com Vue.js 3 e Vuetify 3 para uma interface de usuário responsiva e amigável.
   - Lazy loading foi implementado nas rotas do Vue Router para otimizar o carregamento e melhorar a experiência do usuário.
   - Utiliza `vue-chartjs` para a renderização de gráficos no dashboard.

--- 

## Lista de Bibliotecas de Terceiros Utilizadas

### Backend:
- **Microsoft.EntityFrameworkCore**: ORM para o banco de dados.
- **Microsoft.AspNetCore.Mvc**: Framework para construção de APIs RESTful.
- **Swashbuckle.AspNetCore**: Para geração de documentação Swagger.

### Frontend:
- **Vue.js 3**: Framework para a construção de interfaces de usuário.
- **Vuetify 3**: Biblioteca de componentes para Vue.js, baseada no Material Design.
- **axios**: Para requisições HTTP.
- **vue-chartjs**: Integração com Chart.js para gráficos.
- **@mdi/font**: Conjunto de ícones do Material Design.

---

## Passos para Rodar o Projeto

### Pré-requisitos

1. **Backend**:
   - .NET SDK 8.0 ou superior instalado.
   - Banco de dados PostgreSQL em execução.

2. **Frontend**:
   - Node.js (16.x ou superior) e npm instalados.

### Como Rodar

#### Backend:

1. Navegue até a pasta do backend.
2. Configure a string de conexão no `appsettings.json` (exemplo no campo `DefaultConnection`).
3. Execute os seguintes comandos:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

#### Frontend:

1. Navegue até a pasta do frontend.
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento:
   ```bash
   npm run serve
   ```

---

## O Que Melhoraríamos se Tívéssemos Mais Tempo

1. **Testes Automatizados**:
   - Implementação de testes end-to-end no frontend com **Cypress**.
   - Ampliação de testes de unidade no backend utilizando **xUnit** para mais cenários.

2. **Performance**:
   - Melhorar o desempenho do backend com **caching** para endpoints frequentemente acessados.
   - Implementar otimização no carregamento de recursos estáticos no frontend.

3. **Funcionalidades Adicionais**:
   - Adicionar paginação nativa no backend.
   - Criar rotas adicionais para relatórios específicos no backend.
   - Melhorar as validações de entrada utilizando **FluentValidation**.

4. **Autenticação e Autorização**:
   - Implementar autenticação com **JWT** para maior segurança da API.
   - Adicionar controle de acesso por função (ex: Admin, Professor, Estudante).

5. **Melhoria no Design**:
   - Refinar o layout dos componentes para uma aparência mais moderna e polida.
   - Adicionar transições animadas ao mudar entre rotas.

6. **Documentação**:
   - Criar documentação detalhada de instalação e configuração para desenvolvedores futuros.

