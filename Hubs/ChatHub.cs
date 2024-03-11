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
    public async Task SendMessage(string message, int? chatId, int userId)
    {
        using (var client = new HttpClient())
        {

            if (chatId is null)
            {
                try
                {
#if DEBUG
                    HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://localhost:5001/createnewchat?message={message}&userId={userId}");
#else
                    HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://apiforblazor:5001/createnewchat?message={message}&userId={userId}");
#endif
                    var answer = await responseMessage.Content.ReadAsStringAsync();
                    await Clients.Caller.SendAsync("ReciveAnswer", answer);
                }
                catch
                {
                    await Clients.Caller.SendAsync("ReciveMessage", "что то пошло не так...");
                }
            }
            else
            {
                try
                {
#if DEBUG
                    HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://localhost:5001/addmessagestochat?chatId={chatId}&message={message}");
#else
                    HttpResponseMessage responseMessage =
                            await client.GetAsync($"http://apiforblazor:5001/addmessagestochat?chatId={chatId}&message={message}");
#endif
                    var answer = await responseMessage.Content.ReadAsStringAsync();
                    await Clients.Caller.SendAsync("ReciveAnswer", answer);
                }
                catch
                {

                }
            }




        }

    }

}