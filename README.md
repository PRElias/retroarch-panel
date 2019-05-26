# Retroarch-panel
Website para exibição de dados de jogos de um Retroarch na rede local

![gif](https://github.com/PRElias/images-gifs-readme/raw/master/retroarch-panel.gif?raw=true)

# Usando

- Instale o dotnet core 2.2
- Mantenha seu RaspberryPI ligado e conectado à sua rede Wifi ou cabo.
- Verifique se o seu computador consegue acessar a pasta share do Recalbox, executando o comando abaixo no meu Run (Executar) ```Win + R```

```\\RETROARCH\share```

- Mapeie uma unidade de rede para o caminho acima atribuindo a letra R ¹
- Abra um terminal na pasta onde baixou o projeto e execute o comando abaixo:

```dotnet run```

## Observações

1. O caminho acima é o oficial do Recalbox, mas a aplicação também funciona para retropie. Nesse caso, provavelmente o mapeamento ficará ```\\RETROPIE\roms```, sendo necessário alterar no arqvuio ```appsettings.json``` o parâmetro ```RecalboxShare``` para apenas ```R:\\```

# Funcionalidades

- Pesquisa por todos os campos
- Ordenação
- Exportação para Excel

# Roadmap

Minha ideia é ir aperfeiçoando o projeto aos poucos e incluindo funcionalidades como:
- Conexão ao retroachievments
- Scraper
- Tradução para outras línguas

Quaisquer sugestões ou **merge requests** são bem-vindos e serão atualizados