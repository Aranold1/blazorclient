using Microsoft.AspNetCore.SignalR;
using GptBlazor.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static System.Net.WebRequestMethods;
namespace BlazorSignalRApp.Hubs;

public class ChatHub : Hub
{

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };
    public async Task SendMessage(string message,int? chatId,int userId)
    {
            using(var client = new HttpClient())
            {
                #if DEBUG
                if (chatId is null)
                {
                    try
                    {
                        HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://localhost:5001/createnewchat?message={message}&userId={userId}");

                        var answer = await responseMessage.Content.ReadAsStringAsync();
                        await Clients.Caller.SendAsync("ReciveAnswer", answer);
                    }
                    catch
                    {
                        await Clients.Caller.SendAsync("ReciveMessage", "Что то пошло не так...");
                    }
                }
                else
                {
                    try
                    {
                        HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://localhost:5001/addmessagestochat?chatId={chatId}&message={message}");

                        var answer = await responseMessage.Content.ReadAsStringAsync();
                        await Clients.Caller.SendAsync("ReciveAnswer", answer);
                    }
                    catch
                    {
                        await Clients.Caller.SendAsync("ReciveMessage", "Что то пошло не так...");

                    }
                }
#endif

#if (!DEBUG)
                if (chatId is null)
                {
                    try
                    {
                        HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://apiforblazor:5001/createnewchat?message={message}&userId={userId}");

                        var answer = await responseMessage.Content.ReadAsStringAsync();
                        await Clients.Caller.SendAsync("ReciveAnswer", answer);
                    }
                    catch
                    {
                        await Clients.Caller.SendAsync("ReciveMessage", "Что то пошло не так...");
                    }
                }
                else
                {
                    try
                    {
                        HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://apiforblazor:5001/addmessagestochat?chatId={chatId}&message={message}");

                        var answer = await responseMessage.Content.ReadAsStringAsync();
                        await Clients.Caller.SendAsync("ReciveAnswer", answer);
                    }
                    catch
                    {
                        await Clients.Caller.SendAsync("ReciveMessage", "Что то пошло не так...");
                    }
                }
#endif

        }

    }
	
}