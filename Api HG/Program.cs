using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
class Program
{
    static async Task Main(string[] args)
    {
        // chave
        string chave = "957137c7";

        // URL base da API
        string baseUrl = "https://api.hgbrasil.com/weather?array_limit=5&fields=only_results,city_name,rain,temp,description,cloudiness,forecast,weekday,rain_probability,max,min,date";

        // Parâmetros da requisição
        string woeid = "458495"; // Código da cidade
        /*
         * Araquari = 458495
         * Joinville = 455873
        */

        // Monta a URL com os parâmetros
        string url = $"{baseUrl}&woeid={woeid}&key={chave}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Faz a requisição GET
                HttpResponseMessage response = await client.GetAsync(url);

                // Verifica se a resposta foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    // Lê o conteúdo da resposta
                    string responseBody = await response.Content.ReadAsStringAsync();
                    
                    // Parse o JSON da resposta para um JObject
                    JObject jsonResponse = JObject.Parse(responseBody);

                    Console.WriteLine("Dia atual:");

                    // Exibe as informações principais (como temperatura, cidade e descrição)
                    Console.WriteLine($"Temperatura: {jsonResponse["temp"]}°C");
                    Console.WriteLine($"Data: {jsonResponse["date"]}");
                    Console.WriteLine($"Descrição: {jsonResponse["description"]}");
                    Console.WriteLine($"Cidade: {jsonResponse["city_name"]}");

                    // Exibe a previsão do tempo para os próximos dias
                    Console.WriteLine("\nPrevisão do Tempo:");

                    foreach (var forecast in jsonResponse["forecast"])
                    {
                        Console.WriteLine($"Data: {forecast["date"]}");
                        Console.WriteLine($"Dia da Semana: {forecast["weekday"]}");
                        Console.WriteLine($"Máxima: {forecast["max"]}°C");
                        Console.WriteLine($"Mínima: {forecast["min"]}°C");
                        Console.WriteLine($"Nuvens: {forecast["cloudiness"]}%");
                        Console.WriteLine($"Chuva: {forecast["rain"]}mm");
                        Console.WriteLine($"Probabilidade de Chuva: {forecast["rain_probability"]}%");
                        Console.WriteLine($"Descrição: {forecast["description"]}");
                        Console.WriteLine(); // Nova linha entre cada previsão
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Detalhes do erro:");
                    Console.WriteLine(errorResponse);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
        }
        Console.ReadKey();
    }
}

/*
 * temp - temperatura atual em ºC
date - data da consulta, em fuso horário do local
time - hora da consulta, em fuso horário do local
condition_code - código da condição de tempo atual veja a lista
description - descrição da condição de tempo atual no idioma escolhido
currently - retorna se está de dia ou de noite no idioma escolhido
cid - antigo identificador da cidade, pode não estar presente em alguns casos
city - nome da cidade seguido por uma vírgula (mantido para as libs antigas)
humidity - umidade atual em percentual
cloudiness - nebulosidade em percentual, de 0 a 100 NOVO
rain - volume de chuva em mm na última hora NOVO
wind_speedy - velocidade do vento em km/h
wind_direction - direção do vento em grau NOVO
wind_cardinal - direção do vento em ponto cardeal NOVO
sunrise - nascer do Sol em horário local da cidade
sunset - pôr do Sol em horário local da cidade
moon_phase - fase da Lua veja a lista NOVO
condition_slug - slug da condição de tempo atual veja a lista
city_name - nome da cidade
timezone - fuso horário da cidade
forecast - array com a previsão do tempo para outros dias
date - data da previsão dd/mm
weekday - dia da semana abreviado
max - temperatura máxima em ºC
min - temperatura mínima em ºC
humidity - umidade prevista em percentual
cloudiness - nebulosidade em percentual, de 0 a 100 NOVO
rain - volume de chuva esperado NOVO
rain_probability - probabilidade de chuva em percentual, de 0 a 100 NOVO
wind_speedy - velocidade do vento em km/h NOVO
sunrise - nascer do Sol em horário local da cidade
sunset - pôr do Sol em horário local da cidade
moon_phase - fase da Lua veja a lista NOVO
description - descrição da previsão
condition - slug da condição veja a lista
*/